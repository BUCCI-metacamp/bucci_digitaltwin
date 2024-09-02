using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TableCreator : MonoBehaviour
{
    public float defaultCellWidth = 200f;
    public float spacing = 5f;
    public TMP_FontAsset cellFont; // ��Ʈ ����

    private GameObject tablePanel;
    public GameObject scrollView; // Scroll View ����
    private RectTransform contentRect; // Scroll View Content�� RectTransform
    public Sprite spImage;

    public GameObject CameraObj;
    public Transform buttonTrans;
    public GameObject mainObject;

    public void CreateTable(int rows, int columns, string[] titles, string[,] contents, float cellHeight = -1f)
    {
        if (tablePanel != null) return; // �̹� ���̺��� �����Ǿ����� ��ȯ

        // Scroll View ���� Ȯ��
        if (scrollView == null)
        {
            Debug.LogError("Scroll View is not assigned.");
            return;
        }
        mainObject.gameObject.SetActive(true);
        CameraObj.gameObject.SetActive(false);
        // Scroll View���� Content RectTransform ��������
        contentRect = scrollView.GetComponent<ScrollRect>().content;
        if (contentRect == null)
        {
            Debug.LogError("Scroll View does not have a content RectTransform.");
            return;
        }

        // ���� �г� ����
        tablePanel = new GameObject("TablePanel");
        RectTransform tablePanelRect = tablePanel.AddComponent<RectTransform>();
        tablePanel.AddComponent<CanvasRenderer>();

        // Anchors �� Pivot ����
        tablePanelRect.anchorMin = new Vector2(0.5f, 0.5f);
        tablePanelRect.anchorMax = new Vector2(0.5f, 0.5f);
        tablePanelRect.pivot = new Vector2(0.5f, 0.5f);

        // Grid Layout Group ����
        GridLayoutGroup gridLayout = tablePanel.AddComponent<GridLayoutGroup>();
        float actualCellHeight = cellHeight > 0 ? cellHeight/2f : defaultCellWidth / 2f;
        gridLayout.cellSize = new Vector2(defaultCellWidth, actualCellHeight);
        gridLayout.spacing = new Vector2(spacing, spacing);
        gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;

        // ���̺� ũ�� ����
        float tableWidth = columns * (defaultCellWidth + spacing) - spacing;
        float tableHeight = (rows + 1) * (actualCellHeight + spacing) - spacing; // Ÿ��Ʋ ���� �߰�
        tablePanelRect.sizeDelta = new Vector2(tableWidth, tableHeight);


        // ���ο� ��ġ�� �̵��Ϸ��� ���ϴ� x, y ��ǥ�� �����մϴ�.
        float newX = -800f; // ���ϴ� x ��ġ
        float newY = -50f; // ���ϴ� y ��ġ

        // ��� ������ ���� anchoredPosition�� (0, 0)���� ����
        //tablePanelRect.anchoredPosition = Vector2.zero;
        tablePanelRect.anchoredPosition = new Vector2(newX, newY);

        // Ÿ��Ʋ �� ����
        for (int i = 0; i < columns; i++)
        {
            GameObject cell = new GameObject("TitleCell_" + i);
            cell.transform.SetParent(tablePanel.transform, false);

            Image cellImage = cell.AddComponent<Image>();
            cellImage.color = Color.gray;

            TextMeshProUGUI cellText = new GameObject("Text").AddComponent<TextMeshProUGUI>();
            cellText.transform.SetParent(cell.transform, false);
            cellText.text = titles[i];
            cellText.alignment = TextAlignmentOptions.Center;
            cellText.color = Color.black;

            // ��Ʈ ����
            if (cellFont != null)
            {
                cellText.font = cellFont;
            }

            RectTransform textRect = cellText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
        }

        // ���� �� ����
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                GameObject cell = new GameObject("Cell_" + r + "_" + c);
                cell.transform.SetParent(tablePanel.transform, false);

                Image cellImage = cell.AddComponent<Image>();

                //// �⺻ �� ���� ���� (����)
                //cellImage.color = new Color(0, 0, 0, 0);

                //// �׵θ��� �����ϱ� ���� Sliced Ÿ������ ����
                //cellImage.type = Image.Type.Sliced;

                // "Good"�� ��� �ʷϻ�, �ƴ� ��� ���
                if (contents[r, c] == "Good" || contents[r, c] == "TRUE" || contents[r, c] == "Completed")
                {
                    cellImage.color = Color.green;
                }
                else if(contents[r, c] == "Defective" || contents[r, c] == "FALSE" || contents[r, c] == "Apple" || contents[r, c] == "Drop")
                {
                    cellImage.color = Color.red;
                }
                else if (contents[r, c] == "Banana")
                {
                    cellImage.color = Color.yellow;
                }
                else if (contents[r, c] == "Orange" || contents[r, c] == "Inspecting" || contents[r, c] == "ProductSucess")
                {
                    cellImage.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
                }
                else
                {
                    cellImage.color = Color.white;
                }

                TextMeshProUGUI cellText = new GameObject("Text").AddComponent<TextMeshProUGUI>();
                cellText.transform.SetParent(cell.transform, false);
                cellText.text = contents[r, c];
                cellText.alignment = TextAlignmentOptions.Center;
                cellText.color = Color.black;

                // ��Ʈ ����
                if (cellFont != null)
                {
                    cellText.font = cellFont;
                }

                RectTransform textRect = cellText.GetComponent<RectTransform>();
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.sizeDelta = Vector2.zero;
            }
        }

        // Ȯ�� ��ư �߰�
        GameObject confirmButton = new GameObject("ConfirmButton");

        // Set as child of the canvas or any other parent you want
        confirmButton.transform.SetParent(buttonTrans, false);

        // Set the button position relative to its parent (CanvasScroll)
        RectTransform buttonRect = confirmButton.AddComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(100, 50); // Set size of the button
        buttonRect.anchorMin = new Vector2(0.5f, 0.1f); // Anchor point at the bottom center
        buttonRect.anchorMax = new Vector2(0.5f, 0.1f); // Anchor point at the bottom center
        buttonRect.pivot = new Vector2(0.5f, 0.5f); // Pivot at the center
        buttonRect.anchoredPosition = new Vector2(0, 0); // Set position of the button       

        // Button ������Ʈ�� �̹��� ����
        Button buttonComponent = confirmButton.AddComponent<Button>();
        Image buttonImage = confirmButton.AddComponent<Image>();

        // ��ư�� ������ �Ϲ����� ��ưó�� ���̰� �ϱ� ���� ���� �� ��Ÿ�� ����
        buttonImage.color = new Color(0.9f, 0.9f, 0.9f); // ���� ȸ��
        buttonImage.sprite = spImage;
        buttonImage.type = Image.Type.Sliced; // ��������Ʈ�� ��ư ũ�⿡ ���߱� ���� Sliced Ÿ������ ����

     
        // ��ư �ؽ�Ʈ ����
        TextMeshProUGUI buttonText = new GameObject("Text").AddComponent<TextMeshProUGUI>();
        buttonText.transform.SetParent(confirmButton.transform, false);
        buttonText.text = "Ȯ��";
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.black;


        // ��Ʈ ����
        if (cellFont != null)
        {
            buttonText.font = cellFont;
        }

        

        //RectTransform buttonRect = confirmButton.GetComponent<RectTransform>();
        //buttonRect.sizeDelta = new Vector2(150, 50); // ��ư ũ�� ����
        //buttonRect.anchorMin = new Vector2(0.5f, 0);
        //buttonRect.anchorMax = new Vector2(0.5f, 0);
        //buttonRect.anchoredPosition = new Vector2(0, -60); // ��ġ ����

        buttonComponent.onClick.AddListener(RemoveTable);

        // ���̺��� Scroll View Content�� �ڽ����� ����
        tablePanel.transform.SetParent(contentRect, false);

        // Content ũ�� ����
        contentRect.sizeDelta = new Vector2(tableWidth, tableHeight + buttonRect.sizeDelta.y + 20); // ���� ���� �߰�
    }

    public void RemoveTable()
    {
        if (tablePanel != null)
        {
            Destroy(tablePanel);
            tablePanel = null;
        }

        mainObject.gameObject.SetActive(false);
        CameraObj.gameObject.SetActive(true);
    }
}
