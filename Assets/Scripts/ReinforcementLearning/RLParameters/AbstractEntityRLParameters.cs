using UnityEngine;

public class AbstractEntityRLParameters : MonoBehaviour
{
    [Header("Rewards - General")]
    public float everyFrameReward;
    public float winEpisode;
    public float loseEpisode;
    public float drawEpisode;
    public float getHurt;
}
