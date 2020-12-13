using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class UserAnalytics : MonoBehaviour
{
    void Start()
    {
        Debug.Log("UserID: " + AnalyticsSessionInfo.userId +
        " | SessionState: " + AnalyticsSessionInfo.sessionState +
        " | SessionID: " + AnalyticsSessionInfo.sessionId +
        " | SessionElapsedTime: " + AnalyticsSessionInfo.sessionElapsedTime * 0.001);
        AnalyticsSessionInfo.sessionStateChanged += OnSessionStateChanged;
    }


    void OnSessionStateChanged(AnalyticsSessionState sessionState, long sessionId, long sessionElapsedTime, bool sessionChanged)
    {
        Debug.Log("User ID: " + AnalyticsSessionInfo.userId +
        " | Session State: " + sessionState +
        " | Session ID: " + sessionId +
        " | Session Elapsed Time: " + sessionElapsedTime * 0.001 +
        " | Session Changed: " + sessionChanged);
    }
}
