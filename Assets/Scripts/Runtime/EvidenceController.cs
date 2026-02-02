using System;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceController : MonoBehaviour
{
    [SerializeField] private GameObject fileTemplate;
    [SerializeField] private GameObject hierarchyMaster;
    [SerializeField] private Sprite[] fileTypes;
    
    [SerializeField] private GameObject policeSequencePrefab; 
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private GameObject livestreamWindow;

    private Button policeButton;

    public int evidenceAmmount { get; private set; }

    public void CallPolice()
    {
        GameManager.Instance.moderatorPerms = false;
        
        if (livestreamWindow != null)
        {
            livestreamWindow.SetActive(true);
            livestreamWindow.transform.SetAsLastSibling(); 
        }

        if (policeSequencePrefab != null && canvasTransform != null)
        {
            Instantiate(policeSequencePrefab, canvasTransform);
        }
        else
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Win);
        }
    }

    public void createNewEvidence(String name, Color asertionColor, int type)
    {
        GameObject go = Instantiate(fileTemplate, hierarchyMaster.transform);
        FileGenerator gen = go.GetComponent<FileGenerator>();

        gen.RefreshIcon(fileTypes[type != 911 ? type : 2], name, asertionColor);

        if (type == 911)
        {
            policeButton = go.GetComponent<Button>();
            if (policeButton != null)
            {
                policeButton.onClick.RemoveAllListeners();
                policeButton.onClick.AddListener(CallPolice);
            }
        }

        evidenceAmmount += 1;
    }
}