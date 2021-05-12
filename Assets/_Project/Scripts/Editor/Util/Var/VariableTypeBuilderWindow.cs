using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using UnityEditor;
using UnityEngine;

namespace Util.Var
{
    public class VariableTypeBuilderWindow : EditorWindow
    {
        private const string TARGET_TYPENAME_KEY = "Util.Events.GameEventBuilderWindow.TargetTypeName";
        private const string ROOT_NAMESPACE_KEY = "Util.Events.GameEventBuilderWindow.RootNamespace";
        private const string EVENT_NAMESPACE_KEY = "Util.Events.GameEventBuilderWindow.EventNamespace";
        private const string OBSERVABLE_NAMESPACE_KEY = "Util.Events.GameEventBuilderWindow.ObservableNamespace";

        private const string ROOT_FILEPATH_KEY = "Util.Events.GameEventBuilderWindow.RootClassFilepath";

        public string TargetTypeName;
        public string RootNamespace = "Util.Var";
        public string EventNamespace = "Events";
        public string ObservableNamespace = "Observe";

        public string RootClassFilepath;

        private SerializedObject _so;

        private SerializedProperty _propTargetType;
        private SerializedProperty _propRootNamespace;
        private SerializedProperty _propEventNamespace;
        private SerializedProperty _propObservableNamespace;

        private SerializedProperty _propRootClassFilepath;

        [MenuItem("Tools/GameEventBuilder")]
        [MenuItem("Assets/Create/Custom/new variable...")]
        private static void ShowWindow()
        {
            var window = GetWindow<VariableTypeBuilderWindow>();
            window.titleContent = new GUIContent("Game Event Builder");
            window.Show();
        }

        private string GetVariableClassName(string prefix) => $"{prefix}Variable";
        private string GetObservableVariableClassName(string prefix) => $"Observable{prefix}Variable";

        private string GetVariableReferenceClassName(string prefix) => $"{prefix}Reference";
        private string GetEventClassName(string prefix) => $"{prefix}GameEvent";

        private string GetEventListenerClassName(string prefix) => $"{prefix}GameEventListenerBehaviour";

        private string GetEventReferenceClassName(string prefix) => $"{prefix}EventReference";

        private void OnEnable()
        {
            _so = new SerializedObject(this);
            _propTargetType = _so.FindProperty(nameof(TargetTypeName));
            _propRootNamespace = _so.FindProperty(nameof(RootNamespace));
            _propEventNamespace = _so.FindProperty(nameof(EventNamespace));
            _propObservableNamespace = _so.FindProperty(nameof(ObservableNamespace));
            _propRootClassFilepath = _so.FindProperty(nameof(RootClassFilepath));

            TargetTypeName = EditorPrefs.GetString(TARGET_TYPENAME_KEY, TargetTypeName);
            RootNamespace = EditorPrefs.GetString(ROOT_NAMESPACE_KEY, RootNamespace);
            EventNamespace = EditorPrefs.GetString(EVENT_NAMESPACE_KEY, EventNamespace);
            ObservableNamespace = EditorPrefs.GetString(OBSERVABLE_NAMESPACE_KEY, ObservableNamespace);
            RootClassFilepath = EditorPrefs.GetString(ROOT_FILEPATH_KEY, RootClassFilepath);
        }

        private void OnDisable()
        {
            EditorPrefs.SetString(TARGET_TYPENAME_KEY, TargetTypeName);
            EditorPrefs.SetString(ROOT_NAMESPACE_KEY, RootNamespace);
            EditorPrefs.SetString(EVENT_NAMESPACE_KEY, EventNamespace);
            EditorPrefs.SetString(OBSERVABLE_NAMESPACE_KEY, ObservableNamespace);
            EditorPrefs.SetString(ROOT_FILEPATH_KEY, RootClassFilepath);
        }

        private void OnGUI()
        {
            using (_so.UpdateScope())
            {
                EditorGUILayout.PropertyField(_propTargetType);
                EditorGUILayout.PropertyField(_propRootNamespace);
                EditorGUILayout.PropertyField(_propEventNamespace);
                EditorGUILayout.PropertyField(_propObservableNamespace);

                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.PropertyField(_propRootClassFilepath);
                    LayoutBrowseButton(_propRootClassFilepath);
                }

                if (GUILayout.Button("Generate"))
                {
                    var prefix = ParsePrefix();
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        GenerateClasses(prefix);
                    }
                }

                if (GUILayout.Button("Delete"))
                {
                    var prefix = ParsePrefix();
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        DeleteClassFiles(prefix);
                    }
                }
            }


            // if left mouse clicked then remove focus from our properties
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                GUI.FocusControl(null);
                Repaint();
            }
        }

        private void LayoutBrowseButton(SerializedProperty prop)
        {
            string result = null;

            var openAt = prop.stringValue;
            if (String.IsNullOrEmpty(openAt))
            {
                openAt = Application.dataPath;
            }

            if (GUILayout.Button("Browse", GUILayout.Width(80)))
            {
                result = EditorUtility.OpenFolderPanel("", openAt, "");

                GUI.FocusControl(null);
                Repaint();
            }

            if (String.IsNullOrEmpty(result)) return;

            prop.stringValue = result;
        }

        private string ParsePrefix()
        {
            var prefix = TakeLastWhile(TargetTypeName, c => char.IsLetterOrDigit(c) || c == '_');

            if (String.IsNullOrEmpty(prefix) || !CodeGenerator.IsValidLanguageIndependentIdentifier(prefix))
            {
                Debug.LogError($"Cannot generate classes with prefx = {prefix}");
                return "";
            }

            prefix = char.ToUpper(prefix.First()) + prefix.Substring(1);


            Debug.Log($"GameEventBuilder: Parsed Prefix = {prefix}");

            return prefix;
        }

        private string TakeLastWhile(string str, Func<char, bool> predicate)
        {
            if (!predicate(str[str.Length - 1])) return "";

            var idx = 0;
            for (int i = str.Length - 1; i > -1; i--)
            {
                if (!predicate(str[i]))
                {
                    idx = i + 1;
                    break;
                }
            }

            return str.Substring(idx);
        }

        private void GenerateClasses(string prefix)
        {
            Debug.Log($"Generating classes of prefix {prefix} in {RootClassFilepath}");

            GenerateFile(GetVariableFileDefinition(prefix));
            GenerateFile(GetObservableVariableFileDefinition(prefix));
            GenerateFile(GetEventFileDefinition(prefix));
            GenerateFile(GetEventListenerFileDefinition(prefix));

            AssetDatabase.Refresh();
        }

        public class FileDefinition
        {
            public string Dir;
            public string Name;
            public string Namespace;
            public string[] Imports;
            public ClassDefinition[] Classes;

            public string FilePath => Dir + Path.DirectorySeparatorChar + Name + ".cs";
        }

        public struct ClassDefinition
        {
            public string ClassName;
            public AttributeDefinition[] Attributes;
            public string BaseClass;
        }

        public struct AttributeDefinition
        {
            public string Name;
            public string[] Arguments;
        }

        private FileDefinition GetVariableFileDefinition(string prefix)
        {
            return new FileDefinition
            {
                Dir = RootClassFilepath,
                Name = GetVariableClassName(prefix),
                Namespace = RootNamespace,
                Imports = new[] {"UnityEngine", $"{RootNamespace}.{ObservableNamespace}"},
                Classes = new[]
                {
                    new ClassDefinition
                    {
                        ClassName = GetVariableClassName(prefix),
                        BaseClass = $"Variable<{TargetTypeName}>",
                        Attributes = new []
                        {
                            new AttributeDefinition
                            {
                                Name = "CreateAssetMenu",
                                Arguments = new []{ $"menuName = \"Custom/Variable/{TargetTypeName}\"" }
                            }
                        }
                    },
                    new ClassDefinition
                    {
                        ClassName = GetVariableReferenceClassName(prefix),
                        BaseClass = $"VariableReference<{GetVariableClassName(prefix)}, {GetObservableVariableClassName(prefix)}, {TargetTypeName}>",
                        Attributes = new []
                        {
                            new AttributeDefinition
                            {
                                Name = "System.Serializable"
                            }
                        }
                    }
                }
            };
        }

        private FileDefinition GetObservableVariableFileDefinition(string prefix)
        {
            return new FileDefinition
            {
                Dir = RootClassFilepath + Path.DirectorySeparatorChar + ObservableNamespace,
                Name = GetObservableVariableClassName(prefix),
                Namespace = $"{RootNamespace}.{ObservableNamespace}",
                Imports = new[] {"UnityEngine"},
                Classes = new[]
                {
                    new ClassDefinition
                    {
                        ClassName = GetObservableVariableClassName(prefix),
                        BaseClass = $"ObservableVariable<{TargetTypeName}>",
                        Attributes = new []
                        {
                            new AttributeDefinition
                            {
                                Name = "CreateAssetMenu",
                                Arguments = new []{ $"menuName = \"Custom/Observable/{TargetTypeName}\"" }
                            }
                        }
                    }
                }
            };
        }

        private FileDefinition GetEventFileDefinition(string prefix)
        {
            return new FileDefinition
            {
                Dir = RootClassFilepath + Path.DirectorySeparatorChar + EventNamespace,
                Name = GetEventClassName(prefix),
                Namespace = $"{RootNamespace}.{EventNamespace}",
                Imports = new[] {"UnityEngine", $"{RootNamespace}.{ObservableNamespace}"},
                Classes = new[]
                {
                    new ClassDefinition
                    {
                        ClassName = GetEventClassName(prefix),
                        BaseClass = $"GameEvent<{TargetTypeName}>",
                        Attributes = new []
                        {
                            new AttributeDefinition
                            {
                                Name = "CreateAssetMenu",
                                Arguments = new []{ $"menuName = \"Custom/Event/{TargetTypeName}\"" }
                            }
                        }
                    },
                    new ClassDefinition
                    {
                        ClassName = GetEventReferenceClassName(prefix),
                        BaseClass = $"EventReference<{GetEventClassName(prefix)}, {GetObservableVariableClassName(prefix)}, {TargetTypeName}>",
                        Attributes = new []
                        {
                            new AttributeDefinition
                            {
                                Name = "System.Serializable"
                            }
                        }
                    }
                }
            };
        }

        private FileDefinition GetEventListenerFileDefinition(string prefix)
        {
            return new FileDefinition
            {
                Dir = RootClassFilepath + Path.DirectorySeparatorChar + EventNamespace,
                Name = GetEventListenerClassName(prefix),
                Namespace = $"{RootNamespace}.{EventNamespace}",
                Imports = new[] {"UnityEngine"},
                Classes = new[]
                {
                    new ClassDefinition
                    {
                        ClassName = GetEventListenerClassName(prefix),
                        BaseClass = $"GameEventListenerBehaviour<{TargetTypeName}, {GetEventReferenceClassName(prefix)}>"
                    }
                }
            };
        }

        private void GenerateFile(FileDefinition file)
        {
            if (string.IsNullOrEmpty(file.Dir) || !Directory.Exists(file.Dir))
            {
                Debug.LogError($"Directory '{file.Dir}' does not exist");
                return;
            }

            var classes = file.Classes.Select(clss =>
            {
                var classDecl = new CodeTypeDeclaration(clss.ClassName)
                {
                    TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                };

                if (clss.Attributes != null)
                {
                    foreach (var attribute in clss.Attributes)
                    {
                        var attr = new CodeAttributeDeclaration(attribute.Name);
                        if (attribute.Arguments != null)
                        {
                            foreach (var argument in attribute.Arguments)
                            {
                                attr.Arguments.Add(
                                    new CodeAttributeArgument(new CodeSnippetExpression(argument))
                                );
                            }
                        }

                        classDecl.CustomAttributes.Add(attr);
                    }
                }

                var baseClass = new CodeTypeReference(clss.BaseClass);

                classDecl.BaseTypes.Add(baseClass);

                return classDecl;
            });



            var @namespace = new CodeNamespace
            {
                Name = file.Namespace
            };

            foreach (var clss in classes)
            {
                @namespace.Types.Add(clss);
            }

            foreach (var import in file.Imports)
            {
                @namespace.Imports.Add(new CodeNamespaceImport(import));
            }


            var compileUnit = new CodeCompileUnit
            {
                Namespaces = {@namespace}
            };


            WriteCsFile(file.FilePath, compileUnit);
        }

        private static void WriteCsFile(string path, CodeCompileUnit compileUnit)
        {
            Debug.Log($"Writing file {path}");
            var directory = Path.GetDirectoryName(path);
            if (directory != null)
            {
                Directory.CreateDirectory(directory);
            }

            using var provider = new CSharpCodeProvider();
            using var sw = new StreamWriter(path, false);
            var opt = new CodeGeneratorOptions
            {
                BlankLinesBetweenMembers = false,
                ElseOnClosing = false,
                VerbatimOrder = false,
                BracingStyle = "C",
                IndentString = "    "
            };
            provider.GenerateCodeFromCompileUnit(compileUnit, sw, opt);
        }

        private void DeleteClassFiles(string prefix)
        {
            var paths = new string[]
            {
                GetVariableFileDefinition(prefix).FilePath,
                GetObservableVariableFileDefinition(prefix).FilePath,
                GetEventFileDefinition(prefix).FilePath,
                GetEventListenerFileDefinition(prefix).FilePath,
            };

            foreach (var path in paths)
            {
                Debug.LogWarning($"Removing {path} + meta file");
                File.Delete(path);
                File.Delete(path + ".meta");
            }

            AssetDatabase.Refresh();
        }
    }
}