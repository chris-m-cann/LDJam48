using System;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using LDJam48.Stats;
using UnityEngine;
using Util;
using Util.Var;
using Util.Var.Events;

namespace LDJam48
{
    public class PlayStore : MonoBehaviour
    {
        [SerializeField] private StatT<int> maxDistance;
        [SerializeField] private VoidGameEvent onShowLeaderboardFailed;
        


        private bool _isConnected;

        private void OnEnable()
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            // Activate the Google Play Games platform
            PlayGamesPlatform.Activate();
            Upload();
        }

        public void ShowLeaderboard()
        {
            Upload(success =>
            {
                if (success)
                {
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_global_max_distance);
                }
                else
                {
                    onShowLeaderboardFailed?.Raise();
                }
            });
        }

        public void Upload()
        {
            Upload(_ => { });
        }

        private void Upload(Action<bool> onUploadComplete)
        {
            if (!_isConnected)
            {
                Login(onUploadComplete);
            }
            else
            {
                OnLoginResponse(SignInStatus.Success, onUploadComplete);
            }
        }
        private void Login(Action<bool> onUploadComplete)
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, status => OnLoginResponse(status, onUploadComplete));
        }

        private void OnLoginResponse(SignInStatus status, Action<bool> onUploadComplete)
        {
            if (status == SignInStatus.Success)
            {
                _isConnected = true;
                UploadRun(onUploadComplete);
            }
            else
            {
                Debug.LogError($"sign in failed, status = {status}");
                _isConnected = false;
                onUploadComplete?.Invoke(false);
            }
        }

        private void UploadRun(Action<bool> onUploadComplete)
        {           
            PlayGamesPlatform.Instance.ReportScore(maxDistance.Value, GPGSIds.leaderboard_global_max_distance, sent =>
            {
                // this returns true if it has TRIED to send it. not saying it got there!!!
                // only returns false f your not authenticated!!!
                Debug.Log($"Sent run, distance = {maxDistance.Value}, succeeded = {sent}");
  
                onUploadComplete.Invoke(true);
            });
        }
        



    }
}