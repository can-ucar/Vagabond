using System;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hitText=null;
    public Image blockImage;
    private int _currentHitPoint;
    private CircleCollider2D _collider2D;

    public static event Action<int> RunSFX2; 

    public enum BlockColor
    {
        Capri,
        GoldenYellow,
        Amber,
        VividRaspberry,
        SpringBud,
        ElectricViolet,
        defaultColor,
        newColor,
    }

    public Color[] colors;
    private BlockColor _newBlockColor=BlockColor.defaultColor;
    [SerializeField]private BlockColor _currentBlockColor=BlockColor.newColor;

    public enum BlockType
    {
        None,
        SquareBlock,
        OctagonBlock,
        ExtraBall,
        Money,
        Bomb
    }

    public BlockType currentBlockType;

    void Awake()
    {
        _collider2D = GetComponent<CircleCollider2D>();
    }
    public void AddHealth(int point)
    {
        _currentHitPoint = point;
        WriteHitText();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball") & currentBlockType == BlockType.SquareBlock || currentBlockType == BlockType.OctagonBlock)
        {
            _currentHitPoint--;
            WriteHitText();
            RunSFX2?.Invoke(2);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball") & currentBlockType == BlockType.ExtraBall)
        {
            DataReceiver.SetBallAmount(1);
            UIManager.TextUpdate(UIManager.TextType.Ball,""+DataReceiver.GetBallAmount());
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ball") & currentBlockType == BlockType.Bomb)
        {
            _collider2D.radius = 150;
            ParticleManager.RunParticle(gameObject.transform.position,ParticleManager.ParticleType.Bomb);
            RunSFX2?.Invoke(5);
            Destroy(gameObject,0.5f);
            
        }
        else if (other.gameObject.CompareTag("Ball") & currentBlockType == BlockType.Money)
        {
            DataReceiver.SetMoney(2);
            UIManager.TextUpdate(UIManager.TextType.Money,"x"+DataReceiver.GetMoney());
            RunSFX2?.Invoke(3);
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Block") & currentBlockType == BlockType.Bomb)
        {
            ParticleManager.RunParticle(other.gameObject.transform.position,ParticleManager.ParticleType.Standart);
            RunSFX2?.Invoke(5);
            Destroy(gameObject);
        }
    }

    void WriteHitText()
    {
        if (currentBlockType == BlockType.SquareBlock || currentBlockType == BlockType.OctagonBlock)
        {
            _hitText.text = "" + _currentHitPoint;
            ColorStateSetter();
            if (_currentHitPoint == 0)
            {
                ParticleManager.RunParticle(gameObject.transform.position,ParticleManager.ParticleType.Standart);
                Destroy(gameObject);
            }
        }
    }

    void ColorStateSetter()
    {
        if (_currentHitPoint > 100)
        {
            _newBlockColor = BlockColor.ElectricViolet;
        }

        else if (_currentHitPoint > 50)
        {
            _newBlockColor = BlockColor.SpringBud;
        }

        else if (_currentHitPoint > 30)
        {
            _newBlockColor = BlockColor.VividRaspberry;
        }

        else if (_currentHitPoint > 15)
        {
            _newBlockColor = BlockColor.Amber;
        }

        else if (_currentHitPoint > 5)
        {
            _newBlockColor = BlockColor.GoldenYellow;
        }
        else if (_currentHitPoint >= 1)
        {
            _newBlockColor = BlockColor.Capri;
        }
        if (_currentBlockColor!=_newBlockColor)
        {
            SetBlockColor();
        }
    }

    void SetBlockColor()
    {
        Color blockColor = Color.white;
        switch (_newBlockColor)
        {
            case BlockColor.Capri:
                blockColor = colors[0];
                break;
            case BlockColor.GoldenYellow:
                blockColor = colors[1];
                break;
            case BlockColor.Amber:
                blockColor = colors[2];
                break;
            case BlockColor.VividRaspberry:
                blockColor = colors[3];
                break;
            case BlockColor.SpringBud:
                blockColor = colors[4];
                break;
            case BlockColor.ElectricViolet:
                blockColor = colors[5];
                break;
        }
        _currentBlockColor = _newBlockColor;
        blockImage.color = blockColor;
    }

}
