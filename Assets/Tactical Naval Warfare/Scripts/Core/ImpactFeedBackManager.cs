using UnityEngine;
using MoreMountains.Feedbacks;

public class ImpactFeedBackManager : MonoBehaviour
{
    public static ImpactFeedBackManager Instance;
    public MMF_Player impactFeedback;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayImpact(Vector3 position)
    {
        impactFeedback?.PlayFeedbacks(position);
    }
}