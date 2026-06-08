using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        ShowPressedSprite();
    }

    private void OnMouseUpAsButton()
    {
        ReplayGame();
    }

    private void OnMouseUp()
    {
        ShowNormalSprite();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ShowPressedSprite();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ShowNormalSprite();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ReplayGame();
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShowPressedSprite()
    {
        spriteRenderer.sprite = pressedSprite;
    }

    private void ShowNormalSprite()
    {
        spriteRenderer.sprite = normalSprite;
    }
}
