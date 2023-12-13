using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RouletteManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    Transform tf;
    //���[���b�g�̖{�̕�����RectTransform�B
    [HideInInspector]
    [SerializeField]
    RectTransform elementsRt;
    [Header("�����~�`�̉摜��R�t��")]
    [SerializeField]
    Sprite circleSprite;
    [Header("���[���b�g�̐j�̉摜��R�t��")]
    [SerializeField]
    Sprite clapperSprite;
    [Header("���[���b�g�̗v�f��")]
    [SerializeField]
    [Range(2, 100)]
    int elementCount = 6;
    [Header("���[���b�g�̗v�f�̐F���w�� (�v�f���Ɣz�񐔂𓯂��ɂ��Ă�������)")]
    [SerializeField]
    Color[] elementColors = {
        new Color32(255, 0, 0, 255),
        new Color32(255, 128, 0, 255),
        new Color32(255, 255, 0, 255),
        new Color32(128, 255, 0, 255),
        new Color32(0, 255, 128, 255),
        new Color32(0, 255, 255, 255),
    };
    [Header("���[���b�g�̉摜�T�C�Y")]
    [SerializeField]
    [Range(100, 1024)]
    float rouletteSize = 512;
    [Header("���[���b�g�̐j�̉摜�T�C�Y")]
    [SerializeField]
    [Range(10, 128)]
    float clapperSize = 64;
    //�}�C�i�X�ŋt��]�B
    [Header("��]����X�s�[�h (�b��)")]
    [SerializeField]
    [Range(-1080, 1080)]
    float spinSpeed = 360;
    [HideInInspector]
    [SerializeField]
    float elementRotationToAdd;
    [HideInInspector]
    [SerializeField]
    float elementFillAmount;
    //�R���[�`���Ǘ��p�B
    Coroutine spin;
    [Header("���ʂ̃C���f�b�N�X (elementCount��6�̏ꍇ�A0�`5)")]
    public int resultElementIndex;
    [Header("(��]�I������)�����̒��� (�b)")]
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float inertiaDuration = 0.5f;
    float inertiaElapsedTime;
    //�v�f�ɓ���镶���֌W�B
    //�t�H���g���w�肵�Ȃ��ꍇ�̓f�t�H���g�̕����g����B
    [Header("�����̃t�H���g")]
    [SerializeField]
    TMP_FontAsset font;
    [Header("�v�f���ɓ���镶���̃T�C�Y")]
    [SerializeField]
    [Range(16, 128)]
    int fontSize = 64;
    [Header("�����̐F")]
    [SerializeField]
    Color tmpColor = new Color32(255, 255, 255, 255);
    [Header("�e�v�f�ɕ\������e�L�X�g (�v�f���Ɣz�񐔂𓯂��ɂ��Ă�������)")]
    [SerializeField]
    string[] tmpTexts = new string[] {
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
    };
    //���[���b�g�������Ɉꎞ�I�Ɏg�p�B
    GameObject elementGo;
    RectTransform elementRt;
    Image elementImg;
    GameObject elementTmpGo;
    RectTransform elementTmpRt;
    TextMeshProUGUI elementTmp;
    void Awake()
    {
        if (tf == null)
        {
            tf = transform;
        }
        if (tf.childCount == 0)
        {
            CreateRoulette();
        }
    }
    //���[���b�g�̉�]���X�^�[�g���������ɃR�����ĂԁB
    public void StartSpin()
    {
        //�ꉞ�A���d���s��h�~�B
        if (spin != null)
        {
            StopCoroutine(spin);
        }
        spin = StartCoroutine(Spin());
    }
    IEnumerator Spin()
    {
        while (true)
        {
            elementsRt.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
            //��ʂ��^�b�v(�N���b�N)�ŁA���X�ɒ�~�B
            if (Input.GetMouseButtonDown(0))
            {
                inertiaElapsedTime = 0;
                while (true)
                {
                    inertiaElapsedTime += Time.deltaTime;
                    elementsRt.Rotate(Vector3.forward * Mathf.Lerp(spinSpeed, 0, inertiaElapsedTime / inertiaDuration) * Time.deltaTime);
                    if (inertiaDuration <= inertiaElapsedTime)
                    {
                        resultElementIndex = Mathf.FloorToInt(elementsRt.eulerAngles.z / elementRotationToAdd % elementCount);
                        //������resultElementIndex�ɉ���������������B
                        //�~�܂����̈�̕�����tmpTexts[resultElementIndex]�Ŏ擾�\�B
                        spin = null;
                        yield break;
                    }
                    yield return null;
                }
            }
            yield return null;
        }
    }
    //Create�{�^���������ƁA�e��ݒ�ɉ����ă��[���b�g�Ɛj�����������B
    public void CreateRoulette()
    {
        if (circleSprite == null || clapperSprite == null)
        {
            print("�C���X�y�N�^�[����eSprite��R�t�����Ă��������I");
            return;
        }
        if (tf == null)
        {
            tf = transform;
        }
        elementRotationToAdd = 360.0f / elementCount;
        elementFillAmount = 1.0f / elementCount;
        elementGo = new GameObject("RouletteElements");
        elementsRt = elementGo.AddComponent<RectTransform>();
        elementsRt.SetParent(tf);
        elementsRt.localScale = Vector3.one;
        elementsRt.anchoredPosition = Vector2.zero;
        elementsRt.sizeDelta = new Vector2(rouletteSize, rouletteSize);
        for (int i = 0; i < elementCount; i++)
        {
            elementGo = new GameObject(System.String.Format("RouletteElement{0}", i + 1));
            elementRt = elementGo.AddComponent<RectTransform>();
            elementRt.SetParent(elementsRt);
            elementRt.localScale = Vector3.one;
            elementRt.anchoredPosition = Vector2.zero;
            elementRt.sizeDelta = new Vector2(rouletteSize, rouletteSize);
            elementTmpGo = new GameObject(System.String.Format("Tmp{0}", i + 1));
            elementTmpRt = elementTmpGo.AddComponent<RectTransform>();
            elementTmpRt.SetParent(elementRt);
            elementTmpRt.localScale = Vector3.one;
            elementTmpRt.anchoredPosition = new Vector2(0, rouletteSize / 4 + fontSize / 2);
            elementTmpRt.RotateAround(elementsRt.position, Vector3.back, elementRotationToAdd / 2);
            elementTmp = elementTmpGo.AddComponent<TextMeshProUGUI>();
            if (font != null)
            {
                elementTmp.font = font;
            }
            elementTmp.fontSize = fontSize;
            elementTmp.enableWordWrapping = false;
            elementTmp.alignment = TextAlignmentOptions.Center;
            //�ꉞ�ARaycastTarget�ƁA���b�`�e�L�X�g�𖳌������Ă���(�g���ꍇ�͖߂��Ă�������)�B
            elementTmp.raycastTarget = false;
            elementTmp.richText = false;
            if (i < tmpTexts.Length)
            {
                elementTmp.text = tmpTexts[i];
            }
            elementTmp.color = tmpColor;
            elementRt.Rotate(Vector3.back * (elementRotationToAdd * i));
            elementImg = elementGo.AddComponent<Image>();
            elementImg.sprite = circleSprite;
            elementImg.type = Image.Type.Filled;
            elementImg.fillMethod = Image.FillMethod.Radial360;
            elementImg.fillOrigin = (int)Image.Origin360.Top;
            elementImg.fillAmount = elementFillAmount;
            //�ꉞ�ARaycastTarget�𖳌������Ă���(�g���ꍇ�͖߂��Ă�������)�B
            elementImg.raycastTarget = false;
            if (i < elementColors.Length)
            {
                elementImg.color = elementColors[i];
            }
        }
        elementGo = new GameObject("Clapper");
        elementRt = elementGo.AddComponent<RectTransform>();
        elementRt.SetParent(tf);
        elementRt.localScale = Vector3.one;
        elementRt.anchoredPosition = new Vector2(0, rouletteSize / 2);
        elementRt.sizeDelta = new Vector2(clapperSize, clapperSize);
        elementImg = elementGo.AddComponent<Image>();
        elementImg.sprite = clapperSprite;
    }
}