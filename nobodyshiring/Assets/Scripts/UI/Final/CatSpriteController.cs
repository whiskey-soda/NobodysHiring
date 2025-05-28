using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class CatSpriteController : MonoBehaviour
{
    Image catImage;
    [SerializeField] List<Sprite> catSprites = new List<Sprite>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catImage = GetComponent<Image>();

        if (!catSprites.Contains(catImage.sprite)) { catSprites.Add(catImage.sprite); }

        // change sprite every time the player sleeps
        SleepManager sleepManager = SleepManager.Instance;
        if (sleepManager != null) { sleepManager.sleep.AddListener(ChangeSprite); }
    }

    void ChangeSprite()
    {
        int newSpriteIndex = Random.Range(0, catSprites.Count);

        // reroll sprite to make sure it changes (if possible)
        while (catSprites.Count > 1 && // multiple sprite options
            newSpriteIndex == catSprites.IndexOf(catImage.sprite)) // new sprite is different from current sprite
        {
            newSpriteIndex = Random.Range(0, catSprites.Count);
        }

        catImage.sprite = catSprites[newSpriteIndex];
    }

}
