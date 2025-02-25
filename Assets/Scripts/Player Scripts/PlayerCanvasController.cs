using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasController : MonoBehaviour
{
    [SerializeField] Text _playerHpText;
    [SerializeField] Text _totalCoins;
    [SerializeField] Text _remainingArrows;
    [SerializeField] Image _playerJumpImage;

    public void _SetHpText(int iRemainingHp)
    {
        _playerHpText.text = iRemainingHp.ToString();
    }
    public void _SetArrowText(int iRemainingArrows)
    {
        _remainingArrows.text = iRemainingArrows.ToString();
    }
    public void _SetJumpImage(int iRemainingJumps)
    {
        // if there are more than on jumps this method can be updated
        _playerJumpImage.enabled = iRemainingJumps > 0;
    }
}
