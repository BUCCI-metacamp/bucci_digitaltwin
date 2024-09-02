using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TableCreator : MonoBehaviour
{
    public float defaultCellWidth = 200f;
    public float spacing = 5f;
    public TMP_FontAsset cellFont; // 폰트 설정

    private GameObject tablePanel;
    public GameObject scrollView; // Scroll View 참조
    private RectTransform contentRect; // Scroll View Content의 RectTransform
    public Sprite spImage;

    public GameObject CameraObj;
    public Transform buttonTrans;
    public GameObject mainObject;

    public void CreateTable(int rows, int columns, string[] titles, string[,] contents, float cellHeight = -1f)
    {
        if (tablePanel != null) return; // 이미 테이블이 생성되었으면 반환

        // Scroll View 설정 확인
        if (scrollView == null)
        {
            Debug.LogError("Scroll View is not assigned.");
            return;
        }
        mainObject.gameObject.SetActive(true);
        CameraObj.gameObject.SetActive(false);
        // Scroll View에서 Content RectTransform 가져오기
        contentRect = scrollView.GetComponent<ScrollRect>().content;
        if (contentRect == null)
        {
            Debug.LogError("Scroll View does not have a content RectTransform.");
            return;
        }

        // 메인 패널 생성
        tablePanel = new GameObject("TablePanel");
        RectTransform tablePanelRect = tablePanel.AddComponent<RectTransform>();
        tablePanel.AddComponent<CanvasRenderer>();

        // Anchors 및 Pivot 설정
        tablePanelRect.anchorMin = new Vector2(0.5f, 0.5f);
        tablePanelRect.anchorMax = new Vector2(0.5f, 0.5f);
        tablePanelRect.pivot = new Vector2(0.5f, 0.5f);

        // Grid Layout Group 설정
        GridLayoutGroup gridLayout = tablePanel.AddComponent<GridLayoutGroup>();
        float actualCellHeight = cellHeight > 0 ? cellHeight/2f : defaultCellWidth / 2f;
        gridLayout.cellSize = new Vector2(defaultCellWidth, actualCellHeight);
        gridLayout.spacing = new Vector2(spacing, spacing);
        gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;

        // 테이블 크기 설정
        float tableWidth = columns * (defaultCellWidth + spacing) - spacing;
        float tableHeight = (rows + 1) * (actualCellHeight + spacing) - spacing; // 타이틀 행을 추가
        tablePanelRect.sizeDelta = new Vector2(tableWidth, tableHeight);


        // 새로운 위치로 이동하려면 원하는 x, y 좌표를 설정합니다.
        float newX = -800f; // 원하는 x 위치
        float newY = -50f; // 원하는 y 위치

        // 가운데 정렬을 위해 anchoredPosition을 (0, 0)으로 설정
        //tablePanelRect.anchoredPosition = Vector2.zero;
        tablePanelRect.anchoredPosition = new Vector2(newX, newY);

        // 타이틀 셀 생성
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

            // 폰트 설정
            if (cellFont != null)
            {
                cellText.font = cellFont;
            }

            RectTransform textRect = cellText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
        }

        // 내용 셀 생성
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                GameObject cell = new GameObject("Cell_" + r + "_" + c);
                cell.transform.SetParent(tablePanel.transform, false);

                Image cellImage = cell.AddComponent<Image>();

                //// 기본 셀 배경색 설정 (투명)
                //cellImage.color = new Color(0, 0, 0, 0);

                //// 테두리를 설정하기 위해 Sliced 타입으로 설정
                //cellImage.type = Image.Type.Sliced;

                // "Good"일 경우 초록색, 아닐 경우 흰색
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

                // 폰트 설정
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

        // 확인 버튼 추가
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

        // Button 컴포넌트와 이미지 설정
        Button buttonComponent = confirmButton.AddComponent<Button>();
        Image buttonImage = confirmButton.AddComponent<Image>();

        // 버튼의 외형을 일반적인 버튼처럼 보이게 하기 위해 색상 및 스타일 설정
        buttonImage.color = new Color(0.9f, 0.9f, 0.9f); // 연한 회색
        buttonImage.sprite = spImage;
        buttonImage.type = Image.Type.Sliced; // 스프라이트를 버튼 크기에 맞추기 위해 Sliced 타입으로 설정

     
        // 버튼 텍스트 설정
        TextMeshProUGUI buttonText = new GameObject("Text").AddComponent<TextMeshProUGUI>();
        buttonText.transform.SetParent(confirmButton.transform, false);
        buttonText.text = "확인";
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.black;


        // 폰트 설정
        if (cellFont != null)
        {
            buttonText.font = cellFont;
        }

        

        //RectTransform buttonRect = confirmButton.GetComponent<RectTransform>();
        //buttonRect.sizeDelta = new Vector2(150, 50); // 버튼 크기 조정
        //buttonRect.anchorMin = new Vector2(0.5f, 0);
        //buttonRect.anchorMax = new Vector2(0.5f, 0);
        //buttonRect.anchoredPosition = new Vector2(0, -60); // 위치 조정

        buttonComponent.onClick.AddListener(RemoveTable);

        // 테이블을 Scroll View Content의 자식으로 설정
        tablePanel.transform.SetParent(contentRect, false);

        // Content 크기 설정
        contentRect.sizeDelta = new Vector2(tableWidth, tableHeight + buttonRect.sizeDelta.y + 20); // 여유 공간 추가
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
