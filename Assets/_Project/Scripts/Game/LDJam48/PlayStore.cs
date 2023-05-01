using System;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using LDJam48.Stats;
using UnityEngine;
using Util;
using Util.Var;

namespace LDJam48
{
    public class PlayStore : MonoBehaviour
    {
        [SerializeField] private StatT<int> maxDistance;


        private bool _isConnected;

        private void OnEnable()
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            Upload();
        }

        public void ShowLeaderboard()
        {
            Upload(() => PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_global_max_distance));
        }

        public void Upload()
        {
            Upload(() => { });
        }

        private void Upload(Action onUploadComplete)
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
        private void Login(Action onUploadComplete)
        {
            PlayGamesPlatform.Instance.Authenticate(status => OnLoginResponse(status, onUploadComplete));
        }

        private void OnLoginResponse(SignInStatus status, Action onUploadComplete)
        {
            if (status == SignInStatus.Success)
            {
                _isConnected = true;
                UploadRun(onUploadComplete);
            }
            else
            {
                _isConnected = false;
            }
        }

        private void UploadRun(Action onUploadComplete)
        {           
            PlayGamesPlatform.Instance.ReportScore(maxDistance.Value, GPGSIds.leaderboard_global_max_distance, sent =>
            {
                // this returns true if it has TRIED to send it. not saying it got there!!!
                // only returns false f your not authenticated!!!
                Debug.Log($"Sent run, distance = {maxDistance.Value}, succeeded = {sent}");
  
                onUploadComplete.Invoke();
            });
        }
        



    }
}