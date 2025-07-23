using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class TransformSnapshot
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Transform transform;
    
    public TransformSnapshot(Transform t)
    {
        transform = t;
        position = t.position;
        rotation = t.rotation;
        scale = t.localScale;
    }
    
    public void ApplyToTransform()
    {
        if (transform != null)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
        }
    }
}

[System.Serializable]
public class FrameSnapshot
{
    public float timestamp;
    public List<TransformSnapshot> transformSnapshots = new List<TransformSnapshot>();
    
    public FrameSnapshot(float time)
    {
        timestamp = time;
    }
}

public class TransformRecorder : MonoBehaviour
{
    [Header("Recording Settings")]
    [SerializeField] private float recordingDuration = 5f;
    [SerializeField] private bool includeInactiveChildren;
    
    [Header("Status")]
    [SerializeField, ReadOnly] private int totalFramesRecorded;
    [SerializeField, ReadOnly] private int currentRewindFrame = -1;
    
    private List<FrameSnapshot> _frameHistory = new List<FrameSnapshot>();
    private List<Transform> _trackedTransforms = new List<Transform>();
    
    private void Start()
    {
        RefreshTrackedTransforms();
    }
    
    private void Update()
    {
        RecordCurrentFrame();
        CleanupOldFrames();
    }
    
    private void RefreshTrackedTransforms()
    {
        _trackedTransforms.Clear();
        
        // Add self
        _trackedTransforms.Add(transform);
        
        // Add all children
        Transform[] children = GetComponentsInChildren<Transform>(includeInactiveChildren);
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] != transform) // Don't add self twice
            {
                _trackedTransforms.Add(children[i]);
            }
        }
    }
    
    private void RecordCurrentFrame()
    {
        float currentTime = Time.time;
        FrameSnapshot snapshot = new FrameSnapshot(currentTime);
        
        foreach (Transform t in _trackedTransforms)
        {
            if (t != null)
            {
                snapshot.transformSnapshots.Add(new TransformSnapshot(t));
            }
        }
        
        _frameHistory.Add(snapshot);
        totalFramesRecorded = _frameHistory.Count;
        
        // Reset rewind position when recording new frames (unless we're actively rewinding)
        if (currentRewindFrame == -1 || currentRewindFrame == _frameHistory.Count - 2)
        {
            currentRewindFrame = -1;
        }
    }
    
    private void CleanupOldFrames()
    {
        float cutoffTime = Time.time - recordingDuration;
        
        for (int i = _frameHistory.Count - 1; i >= 0; i--)
        {
            if (_frameHistory[i].timestamp < cutoffTime)
            {
                _frameHistory.RemoveAt(i);
            }
            else
            {
                break; // Since frames are added chronologically, we can stop here
            }
        }
        
        totalFramesRecorded = _frameHistory.Count;
    }
    
    [Button("Rewind to Start")]
    public void RewindToStart()
    {
        if (_frameHistory.Count > 0)
        {
            currentRewindFrame = 0;
            ApplyFrameSnapshot(_frameHistory[0]);
            Debug.Log($"Rewound to start (frame 0/{_frameHistory.Count - 1})");
        }
    }
    
    [Button("Rewind One Frame")]
    public void RewindOneFrame()
    {
        if (_frameHistory.Count > 0)
        {
            if (currentRewindFrame == -1)
            {
                currentRewindFrame = _frameHistory.Count - 1;
            }
            else if (currentRewindFrame > 0)
            {
                currentRewindFrame--;
            }
            
            ApplyFrameSnapshot(_frameHistory[currentRewindFrame]);
            Debug.Log($"Rewound to frame {currentRewindFrame}/{_frameHistory.Count - 1}");
        }
    }
    
    [Button("Forward One Frame")]
    public void ForwardOneFrame()
    {
        if (_frameHistory.Count > 0 && currentRewindFrame != -1)
        {
            if (currentRewindFrame < _frameHistory.Count - 1)
            {
                currentRewindFrame++;
                ApplyFrameSnapshot(_frameHistory[currentRewindFrame]);
                Debug.Log($"Advanced to frame {currentRewindFrame}/{_frameHistory.Count - 1}");
            }
        }
    }
    
    [Button("Rewind to End")]
    public void RewindToEnd()
    {
        if (_frameHistory.Count > 0)
        {
            currentRewindFrame = _frameHistory.Count - 1;
            ApplyFrameSnapshot(_frameHistory[currentRewindFrame]);
            Debug.Log($"Rewound to end (frame {currentRewindFrame}/{_frameHistory.Count - 1})");
        }
    }
    
    [Button("Reset to Live")]
    public void ResetToLive()
    {
        currentRewindFrame = -1;
        Debug.Log("Reset to live mode - transforms will follow normal behavior");
    }
    
    [Button("Clear History")]
    public void ClearHistory()
    {
        _frameHistory.Clear();
        totalFramesRecorded = 0;
        currentRewindFrame = -1;
        Debug.Log("Cleared transform history");
    }
    
    private void ApplyFrameSnapshot(FrameSnapshot snapshot)
    {
        foreach (TransformSnapshot transformSnapshot in snapshot.transformSnapshots)
        {
            transformSnapshot.ApplyToTransform();
        }
    }
    
    [Button("Refresh Tracked Transforms")]
    public void RefreshTrackedTransformsButton()
    {
        RefreshTrackedTransforms();
        Debug.Log($"Refreshed tracked transforms. Now tracking {_trackedTransforms.Count} objects");
    }
    
    // Additional utility methods
    public void SetRecordingDuration(float duration)
    {
        recordingDuration = Mathf.Max(0.1f, duration);
    }
    
    public int GetFrameCount()
    {
        return _frameHistory.Count;
    }
    
    public int GetCurrentRewindFrame()
    {
        return currentRewindFrame;
    }
    
    public float GetRecordingDuration()
    {
        return recordingDuration;
    }
    
    private void OnDrawGizmos()
    {
        if (currentRewindFrame != -1)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }
    }
}
