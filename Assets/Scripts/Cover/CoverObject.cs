using UnityEngine;

public class CoverObject : MonoBehaviour
{
    [SerializeField] Cover.CoverType coverType = Cover.CoverType.HalfCover;

    public Cover.CoverType GetCoverType()
    {
        return coverType;
    }
}
