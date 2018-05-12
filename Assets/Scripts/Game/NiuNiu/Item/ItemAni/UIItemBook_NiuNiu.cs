//===================================================
//Author      : WZQ
//CreateTime  ：10/16/2017 1:18:13 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace NiuNiu
{
    public enum FlipMode
    {       
        RightDownToRight,
        RightMiddleToRight,
        RightUpToRight,
        LeftDownToRight,
        LeftMiddleToRight,
        LeftUpToRight,
    }
    public class UIItemBook_NiuNiu : UIItemBase
    {
        public Canvas canvas;
        [SerializeField]
        RectTransform BookPanel;
        public Sprite background;
        public Sprite[] bookPages;
        
        public bool interactable = true;
        public bool enableShadowEffect = true;

        public int currentPage = 0;

        public Image ClippingPlane;
        public Image NextPageClip;
        public Image Shadow;
        public Image ShadowLTR;
        public Image Left;
        public Image LeftNext;
        public Image Right;
        public Image RightNext;

        public UnityEvent OnFlip;
        float radius1, radius2;

        //Spine Bottom
        Vector3 sb;
        //Spine Top
        Vector3 st;
        //corner of the page
        Vector3 c;

        //Edge Bottom Right
        Vector3 ebr;
        //Edge Bottom Left
        Vector3 ebl;

        Vector3 etr;

        Vector3 etl;

        //follow point 
        Vector3 f;
        bool pageDragging = false;
        FlipMode mode;


        /// <summary>
        /// 初始化书
        /// </summary>
        /// <param name="bookPages"></param>
        public void InitBook(Sprite[] bookPages,int currentPage,Canvas canvas)
        {
            RightNext.gameObject.SetActive(false);
            this.bookPages = bookPages;
            this.currentPage = currentPage;
            this.canvas = canvas;

            float scaleFactor = 1;
            if (canvas) scaleFactor = canvas.scaleFactor;
            float pageWidth = (BookPanel.rect.width * scaleFactor - 1) / 2;   
            float pageHeight = BookPanel.rect.height * scaleFactor;          
            Left.gameObject.SetActive(false);
            Right.gameObject.SetActive(false);
            UpdateSprites();

            //Vector3 globalsb = BookPanel.transform.position + new Vector3(0, -pageHeight / 2);         
            //sb = transformPoint(globalsb);
            sb = new Vector3(0, -BookPanel.rect.height / 2);


            //Vector3 globalebr = BookPanel.transform.position + new Vector3(pageWidth, -pageHeight / 2);  
            //ebr = transformPoint(globalebr);
            ebr = new Vector3((BookPanel.rect.width - 1) / 2, -BookPanel.rect.height / 2);

            etr = new Vector3((BookPanel.rect.width - 1) / 2, BookPanel.rect.height / 2);                  

            //Vector3 globalebl = BookPanel.transform.position + new Vector3(-pageWidth, -pageHeight / 2); 
            //ebl = transformPoint(globalebl);
            ebl = new Vector3(-(BookPanel.rect.width - 1) / 2, -BookPanel.rect.height / 2);

            etl = new Vector3(-(BookPanel.rect.width - 1) / 2, BookPanel.rect.height / 2);                 

            //Vector3 globalst = BookPanel.transform.position + new Vector3(0, pageHeight / 2);         
            //st = transformPoint(globalst);
            st = new Vector3(0, BookPanel.rect.height / 2);

            radius1 = Vector2.Distance(sb, ebr);
            float scaledPageWidth = pageWidth / scaleFactor;
            float scaledPageHeight = pageHeight / scaleFactor;
            radius2 = Mathf.Sqrt(scaledPageWidth * scaledPageWidth + scaledPageHeight * scaledPageHeight);
            ClippingPlane.rectTransform.sizeDelta = new Vector2(scaledPageWidth * 2, scaledPageHeight + scaledPageWidth * 2);
            Shadow.rectTransform.sizeDelta = new Vector2(scaledPageWidth, scaledPageHeight + scaledPageWidth * 0.6f);
            ShadowLTR.rectTransform.sizeDelta = new Vector2(scaledPageWidth, scaledPageHeight + scaledPageWidth * 0.6f);
            NextPageClip.rectTransform.sizeDelta = new Vector2(scaledPageWidth, scaledPageHeight + scaledPageWidth * 0.6f);

        }



        // Update is called once per frame
        void Update()
        {

            if (pageDragging && interactable)
            {
                UpdateBook();

                float distanceToLeft = Vector2.Distance(c, ebl);
                float distanceToRight = Vector2.Distance(c, ebr);

                Debug.Log(string.Format("c:{0} distanceToLeft:{1} distanceToRight:{2}", c, distanceToLeft, distanceToRight));

                if (mode == FlipMode.LeftDownToRight || mode == FlipMode.LeftMiddleToRight || mode == FlipMode.LeftUpToRight)
                {
                    if (distanceToRight < distanceToLeft) ReleasePage();
                }
                else
                {
                    if (distanceToRight > distanceToLeft) ReleasePage();
                }

            }
        }

        //更新
        public void UpdateBook()
        {

            Vector2 _pos = Vector2.one;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                        Input.mousePosition, canvas.worldCamera, out _pos);
            _pos = canvas.transform.TransformPoint(_pos);

            f = Vector3.Lerp(f, transformPoint(_pos), Time.deltaTime * 50);


            if (mode == FlipMode.RightDownToRight || mode == FlipMode.RightMiddleToRight || mode == FlipMode.RightUpToRight)
                UpdateBookRTLToPoint(f);
            else
                UpdateBookLTRToPoint(f);

        }

        
        public void OnMouseDragLeftPage()
        {
            if (interactable)
            {
                //DragLeftPageToPoint(transformPoint(Input.mousePosition));

                mode = FlipMode.LeftDownToRight;

                LeftNext.gameObject.SetActive(false);
                Vector2 _pos = Vector2.one;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                            Input.mousePosition, canvas.worldCamera, out _pos);
                _pos = canvas.transform.TransformPoint(_pos);
                DragLeftPageToPoint(transformPoint(_pos));

            }

        }

        
        public void OnMouseDragLeftMiddlePage()
        {
            if (interactable)
            {
                mode = FlipMode.LeftMiddleToRight;

                LeftNext.gameObject.SetActive(false);
                Vector2 _pos = Vector2.one;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                            Input.mousePosition, canvas.worldCamera, out _pos);
                _pos = canvas.transform.TransformPoint(_pos);
                DragLeftPageToPoint(transformPoint(_pos));
                //DragLeftMiddlePageToPoint(transformPoint(_pos));

            }

        }

       
        public void OnMouseDragLeftUPPage()
        {
            if (interactable)
            {
                mode = FlipMode.LeftUpToRight;

                LeftNext.gameObject.SetActive(false);
                Vector2 _pos = Vector2.one;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                            Input.mousePosition, canvas.worldCamera, out _pos);
                _pos = canvas.transform.TransformPoint(_pos);
                DragLeftPageToPoint(transformPoint(_pos));
            }

        }




        
        public void OnMouseDragRightPage()
        {
            if (interactable)
            {
                mode = FlipMode.RightDownToRight;

                //DragRightPageToPoint(transformPoint(Input.mousePosition));

                Vector2 _pos = Vector2.one;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                            Input.mousePosition, canvas.worldCamera, out _pos);
                _pos = canvas.transform.TransformPoint(_pos);
                DragRightPageToPoint(transformPoint(_pos));
            }

        }

        #region 从左往右拖拽中各项设置

        //计算从左拖各点坐标
        public void DragLeftPageToPoint(Vector3 point)
        {
            if (currentPage <= 0) return;//当前为第0页
            pageDragging = true;

            //mode = FlipMode.LeftDownToRight;
            f = point;

            NextPageClip.rectTransform.pivot = new Vector2(1, 0.12f);
            ClippingPlane.rectTransform.pivot = new Vector2(0, 0.5f);

            Right.gameObject.SetActive(true);
            Right.transform.position = LeftNext.transform.position;
            Right.sprite = bookPages[currentPage - 1];

            //Right.transform.eulerAngles = new Vector3(0, 0, 0);
            Right.transform.eulerAngles = BookPanel.eulerAngles;

            Right.transform.SetAsFirstSibling();

            Left.gameObject.SetActive(true);
            Left.rectTransform.pivot = new Vector2(1, 0);
            Left.transform.position = LeftNext.transform.position;

            //Left.transform.eulerAngles = new Vector3(0, 0, 0);
            Left.transform.eulerAngles = BookPanel.eulerAngles;


            Left.sprite = (currentPage >= 2) ? bookPages[currentPage - 2] : background; 

            LeftNext.sprite = (currentPage >= 3) ? bookPages[currentPage - 3] : background; 

            RightNext.transform.SetAsFirstSibling();
            if (enableShadowEffect) ShadowLTR.gameObject.SetActive(true);


            if (mode == FlipMode.LeftDownToRight)
            {
                Left.rectTransform.pivot = new Vector2(1, 0);
                ShadowLTR.rectTransform.pivot = new Vector2(0, 0.1f);
            }
            else if (mode == FlipMode.LeftUpToRight)
            {
                Left.rectTransform.pivot = new Vector2(1, 1);
                ShadowLTR.rectTransform.pivot = new Vector2(0, 0.9f);
            }
            else
            {
                Left.rectTransform.pivot = new Vector2(1, 0);
                ShadowLTR.rectTransform.pivot = new Vector2(0, 0.1f);
            }

            UpdateBookLTRToPoint(f);
        }


        //计算从右拖各店坐标
        public void DragRightPageToPoint(Vector3 point)
        {
            if (currentPage >= bookPages.Length) return;
            pageDragging = true;
            //mode = FlipMode.RightToLeft;
            f = point;


            NextPageClip.rectTransform.pivot = new Vector2(0, 0.12f);
            ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);

            Left.gameObject.SetActive(true);
            Left.rectTransform.pivot = new Vector2(0, 0);
            Left.transform.position = RightNext.transform.position;
            //Left.transform.eulerAngles = new Vector3(0, 0, 0);
            Left.transform.eulerAngles = BookPanel.eulerAngles;
            
            Left.sprite = (currentPage < bookPages.Length) ? bookPages[currentPage] : background;
            Left.transform.SetAsFirstSibling();

            Right.gameObject.SetActive(true);
            Right.transform.position = RightNext.transform.position;
            //Right.transform.eulerAngles = new Vector3(0, 0, 0);
            Right.transform.eulerAngles = BookPanel.eulerAngles;
            Right.sprite = (currentPage < bookPages.Length - 1) ? bookPages[currentPage + 1] : background;

            RightNext.sprite = (currentPage < bookPages.Length - 2) ? bookPages[currentPage + 2] : background;

            LeftNext.transform.SetAsFirstSibling();
            if (enableShadowEffect) Shadow.gameObject.SetActive(true);
            UpdateBookRTLToPoint(f);
        }
        #endregion



        //返回相对书坐标
        public Vector3 transformPoint(Vector3 global)
        {
            Vector2 localPos = BookPanel.InverseTransformPoint(global);
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(BookPanel, global, null, out localPos);
            return localPos;
        }

        #region 更新位置




        #region L T R 左向右

        /// <summary>
        /// 由左向右翻页时 更新位置
        /// </summary>
        /// <param name="followLocation"></param>
        public void UpdateBookLTRToPoint(Vector3 followLocation)
        {
          
            f = followLocation;
            ShadowLTR.transform.SetParent(ClippingPlane.transform, true);
            ShadowLTR.transform.localPosition = new Vector3(0, 0, 0);
            ShadowLTR.transform.localEulerAngles = new Vector3(0, 0, 0);
            Left.transform.SetParent(ClippingPlane.transform, true);

            Right.transform.SetParent(BookPanel.transform, true);
            LeftNext.transform.SetParent(BookPanel.transform, true);


            c = Calc_C_Position(followLocation);
            Vector3 t1;
            float T0_T1_Angle;
            if (mode == FlipMode.LeftDownToRight)
            {
                T0_T1_Angle = Calc_T0_T1_Angle(c, ebl, out t1);
            }
            else if (mode == FlipMode.LeftMiddleToRight)
            {
                T0_T1_Angle = Calc_T0_T1_Angle(c, ebl, out t1);
            }
            else
            {
                 T0_T1_Angle = Calc_T0_T1_Angle(c, etl, out t1);           
            }

          
            if (T0_T1_Angle < 0) T0_T1_Angle += 180;


            ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, T0_T1_Angle - 90);
            ClippingPlane.transform.localPosition = t1;

            //page position and angle
            Left.transform.position = BookPanel.TransformPoint(c);                           


            float C_T1_Angle = Calc_C_T1_Angle(c, t1);

            //Left.transform.eulerAngles = new Vector3(0, 0, C_T1_Angle - 180);
            Left.transform.eulerAngles = new Vector3(0, 0, C_T1_Angle - 180) + BookPanel.eulerAngles;

            //NextPageClip.transform.eulerAngles = new Vector3(0, 0, T0_T1_Angle - 90);
            NextPageClip.transform.eulerAngles = new Vector3(0, 0, T0_T1_Angle - 90) + BookPanel.eulerAngles;
            

            NextPageClip.transform.position = BookPanel.TransformPoint(t1);
            LeftNext.transform.SetParent(NextPageClip.transform, true);
            Right.transform.SetParent(ClippingPlane.transform, true);
            Right.transform.SetAsFirstSibling();

            ShadowLTR.rectTransform.SetParent(Left.rectTransform, true);                    


        }

        #endregion




        #region R T L  由右向左 更新位置

        /// <summary>
        /// 由右向左翻页时 更新位置
        /// </summary>
        /// <param name="followLocation"></param>
        public void UpdateBookRTLToPoint(Vector3 followLocation)
        {
            mode = FlipMode.RightDownToRight;
            f = followLocation;
            Shadow.transform.SetParent(ClippingPlane.transform, true);
            Shadow.transform.localPosition = new Vector3(0, 0, 0);
            Shadow.transform.localEulerAngles = new Vector3(0, 0, 0);
            Right.transform.SetParent(ClippingPlane.transform, true);

            Left.transform.SetParent(BookPanel.transform, true);
            RightNext.transform.SetParent(BookPanel.transform, true);
            c = Calc_C_Position(followLocation);
            Vector3 t1;
            float T0_T1_Angle = Calc_T0_T1_Angle(c, ebr, out t1);
            if (T0_T1_Angle >= -90) T0_T1_Angle -= 180;

            ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);

            //ClippingPlane.transform.eulerAngles = new Vector3(0, 0, T0_T1_Angle + 90);
            ClippingPlane.transform.eulerAngles = new Vector3(0, 0, T0_T1_Angle + 90) + BookPanel.eulerAngles;
            ClippingPlane.transform.position = BookPanel.TransformPoint(t1);

            //page position and angle
            Right.transform.position = BookPanel.TransformPoint(c);
            float C_T1_dy = t1.y - c.y;
            float C_T1_dx = t1.x - c.x;
            float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
            Right.transform.eulerAngles = new Vector3(0, 0, C_T1_Angle) + BookPanel.eulerAngles;

            //NextPageClip.transform.eulerAngles = new Vector3(0, 0, T0_T1_Angle + 90);
            NextPageClip.transform.eulerAngles = new Vector3(0, 0, T0_T1_Angle + 90) + BookPanel.eulerAngles;
            NextPageClip.transform.position = BookPanel.TransformPoint(t1);
            RightNext.transform.SetParent(NextPageClip.transform, true);
            Left.transform.SetParent(ClippingPlane.transform, true);
            Left.transform.SetAsFirstSibling();

            Shadow.rectTransform.SetParent(Right.rectTransform, true);
        }

        #endregion


        #endregion











        #region  c
        
        private Vector3 Calc_C_Position(Vector3 followLocation)
        {
            Vector3 c;
            f = followLocation;

            //是否是从上翻
            if (mode == FlipMode.LeftUpToRight || mode == FlipMode.RightUpToRight)
            {
                float F_ST_dy = f.y - st.y;
                float F_ST_dx = f.x - st.x;
                float F_ST_Angle = Mathf.Atan2(F_ST_dy, F_ST_dy);
                Vector3 r1 = new Vector3(radius1 * Mathf.Cos(F_ST_Angle), radius1 * Mathf.Sin(F_ST_Angle), 0) + st;

                float F_ST_distance = Vector2.Distance(f, st);
                if (F_ST_distance < radius1)
                    c = f;
                else
                    c = r1;
                float F_SB_dy = c.y - sb.y;
                float F_SB_dx = c.x - sb.x;
                float F_SB_Angle = Mathf.Atan2(F_SB_dy, F_SB_dy);
                Vector3 r2 = new Vector3(radius2 * Mathf.Cos(F_SB_Angle),
                   radius2 * Mathf.Sin(F_SB_Angle), 0) + sb;
                float C_SB_distance = Vector2.Distance(c, sb);
                if (C_SB_distance > radius2)
                    c = r2;
            }
            else if (mode == FlipMode.LeftMiddleToRight || mode == FlipMode.RightMiddleToRight)
            {
                c.x = Mathf.Clamp(f.x, ebl.x, ebr.x);
                c.y = sb.y;
                c.z = 0;
            }

            else
            {
                float F_SB_dy = f.y - sb.y;
                float F_SB_dx = f.x - sb.x;
                float F_SB_Angle = Mathf.Atan2(F_SB_dy, F_SB_dx);
                Vector3 r1 = new Vector3(radius1 * Mathf.Cos(F_SB_Angle), radius1 * Mathf.Sin(F_SB_Angle), 0) + sb;

                float F_SB_distance = Vector2.Distance(f, sb);
                if (F_SB_distance < radius1)
                    c = f;
                else
                    c = r1;
                float F_ST_dy = c.y - st.y;
                float F_ST_dx = c.x - st.x;
                float F_ST_Angle = Mathf.Atan2(F_ST_dy, F_ST_dx);                //c点与上脊柱点角度
                Vector3 r2 = new Vector3(radius2 * Mathf.Cos(F_ST_Angle),
                   radius2 * Mathf.Sin(F_ST_Angle), 0) + st;
                float C_ST_distance = Vector2.Distance(c, st);
                if (C_ST_distance > radius2)
                    c = r2;

            }
            return c;
        }
        #endregion

        #region  计算角度
        //计算c点角度
        private float Calc_T0_T1_Angle(Vector3 c, Vector3 bookCorner, out Vector3 t1)
        {
            Vector3 t0 = (c + bookCorner) / 2;                                      //t0 c与角落点的中点 切线中点
            float T0_CORNER_dy = bookCorner.y - t0.y;                               //
            float T0_CORNER_dx = bookCorner.x - t0.x;
            float T0_CORNER_Angle = Mathf.Atan2(T0_CORNER_dy, T0_CORNER_dx);        //T0_CORNER_Angle 角落点以C点为原点 的角度
            float T0_T1_Angle = 90 - T0_CORNER_Angle;

            if (mode == FlipMode.LeftUpToRight || mode== FlipMode.RightUpToRight)
            {
                float T1_X = t0.x - T0_CORNER_dy * Mathf.Tan(T0_CORNER_Angle);
                T1_X = normalizeT1X(T1_X, bookCorner, st);
                t1 = new Vector3(T1_X, st.y, 0);
            }
            else if (mode == FlipMode.LeftMiddleToRight || mode == FlipMode.RightMiddleToRight)
            {
                t1 = new Vector3((c.x + bookCorner.x) * 0.5f, bookCorner.y, 0);
                t1.x = Mathf.Clamp(t1.x, bookCorner.x, sb.x);
                //float T0_T1_Angle = Mathf.Atan2(1, 0) * Mathf.Rad2Deg;
            }
            else
            {
                float T1_X = t0.x - T0_CORNER_dy * Mathf.Tan(T0_CORNER_Angle);
                T1_X = normalizeT1X(T1_X, bookCorner, sb);
                t1 = new Vector3(T1_X, sb.y, 0);                                         //T1  切线与下底面交点

            }



            ////////////////////////////////////////////////
            //clipping plane angle=T0_T1_Angle
            float T0_T1_dy = t1.y - t0.y;
            float T0_T1_dx = t1.x - t0.x;
            T0_T1_Angle = Mathf.Atan2(T0_T1_dy, T0_T1_dx) * Mathf.Rad2Deg;

            if (mode == FlipMode.LeftMiddleToRight || mode == FlipMode.RightMiddleToRight) T0_T1_Angle = -90;

            return T0_T1_Angle;
        }
        private float normalizeT1X(float t1, Vector3 corner, Vector3 sb)
        {
            if (t1 > sb.x && sb.x > corner.x)
                return sb.x;
            if (t1 < sb.x && sb.x < corner.x)
                return sb.x;
            return t1;
        }
        #endregion

        private float Calc_C_T1_Angle(Vector3 c,  Vector3 t1)
        {
            float C_T1_dy = t1.y - c.y;
            float C_T1_dx = t1.x - c.x;
            float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;

            if (mode == FlipMode.LeftMiddleToRight || mode == FlipMode.RightMiddleToRight)
                C_T1_Angle = 180;
            return C_T1_Angle;
        }



        #region  结束拖拽

        //更新全部图片
        void UpdateSprites()
        {
            LeftNext.sprite = (currentPage > 0 && currentPage <= bookPages.Length) ? bookPages[currentPage - 1] : background;
            RightNext.sprite = (currentPage >= 0 && currentPage < bookPages.Length) ? bookPages[currentPage] : background;
        }


        //结束拖拽监听
        public void OnMouseRelease()
        {
            if (interactable)
                ReleasePage();
        }

        public void ReleasePage()
        {
            if (pageDragging)
            {
                pageDragging = false;
                float distanceToLeft = Vector2.Distance(c, ebl);
                float distanceToRight = Vector2.Distance(c, ebr);

                if (distanceToRight < distanceToLeft &&(mode == FlipMode.RightDownToRight || mode == FlipMode.RightMiddleToRight || mode == FlipMode.RightUpToRight) )
                    TweenBack();
                else if (distanceToRight > distanceToLeft && (mode == FlipMode.LeftDownToRight || mode == FlipMode.LeftMiddleToRight || mode == FlipMode.LeftUpToRight))
                    TweenBack();
                else
                    TweenForward();
                
            }
        }

        Coroutine currentCoroutine;//当前协同程序

        /// <summary>
        /// 翻书失败
        /// </summary>
        public void TweenBack()
        {
            Vector3 endPos;

            if (mode == FlipMode.RightDownToRight || mode == FlipMode.RightMiddleToRight)
                endPos = ebr;
            else if (mode == FlipMode.RightUpToRight)
                endPos = etr;
            else if (mode == FlipMode.LeftUpToRight)
                endPos = etl;
            else
                endPos = ebl;

      
            if (mode == FlipMode.RightDownToRight || mode == FlipMode.RightMiddleToRight || mode == FlipMode.RightUpToRight)
            {
                currentCoroutine = StartCoroutine(TweenTo(endPos, 0.1f,
                    () =>
                    {
                        UpdateSprites();
                        RightNext.transform.SetParent(BookPanel.transform);
                        Right.transform.SetParent(BookPanel.transform);

                        Left.gameObject.SetActive(false);
                        Right.gameObject.SetActive(false);
                        pageDragging = false;
                    }
                    ));
            }
            else
            {
                currentCoroutine = StartCoroutine(TweenTo(endPos, 0.1f,
                    () =>
                    {
                        UpdateSprites();

                        LeftNext.transform.SetParent(BookPanel.transform);
                        Left.transform.SetParent(BookPanel.transform);

                        Left.gameObject.SetActive(false);
                        Right.gameObject.SetActive(false);
                        pageDragging = false;

                        LeftNext.gameObject.SetActive(true);//,,,
                    }
                    ));
            }
        }


        /// <summary>
        /// 翻书成功
        /// </summary>
        public void TweenForward()
        {
            Vector3 endPos;


            if (mode == FlipMode.RightDownToRight || mode == FlipMode.RightMiddleToRight)
                endPos = ebl;
            else if (mode == FlipMode.RightUpToRight)
                endPos = etl;
            else if (mode == FlipMode.LeftUpToRight)
                endPos = etr;
            else
                endPos = ebr;
            currentCoroutine = StartCoroutine(TweenTo(endPos, 0.1f, () => { Flip(); }));
        }


        /// <summary>
        /// 翻页结束事件
        /// </summary>
        void Flip()
        {
            if (mode == FlipMode.RightDownToRight || mode == FlipMode.RightMiddleToRight || mode == FlipMode.RightUpToRight)
                currentPage += 2;
            else
            {
                RightNext.gameObject.SetActive(true);//,,,
                currentPage -= 2;
            }
            LeftNext.transform.SetParent(BookPanel.transform, true);
            Left.transform.SetParent(BookPanel.transform, true);
            LeftNext.transform.SetParent(BookPanel.transform, true);
            Left.gameObject.SetActive(false);
            Right.gameObject.SetActive(false);
            Right.transform.SetParent(BookPanel.transform, true);
            RightNext.transform.SetParent(BookPanel.transform, true);
            UpdateSprites();
            Shadow.gameObject.SetActive(false);
            ShadowLTR.gameObject.SetActive(false);
            if (OnFlip != null)
                OnFlip.Invoke();
        }

        /// <summary>
        /// 自动翻完剩余动画
        /// </summary>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <param name="onFinish"></param>
        /// <returns></returns>
        public IEnumerator TweenTo(Vector3 to, float duration, System.Action onFinish)
        {
            int steps = (int)(duration / 0.01f);
            Vector3 displacement = (to - f) / steps;
            for (int i = 0; i < steps - 1; i++)
            {
                if (mode == FlipMode.RightDownToRight || mode == FlipMode.RightMiddleToRight || mode == FlipMode.RightUpToRight)
                    UpdateBookRTLToPoint(f + displacement);
                else
                    UpdateBookLTRToPoint(f + displacement);

                yield return new WaitForSeconds(0.01f);
            }
            if (onFinish != null)
                onFinish();
        }
        #endregion
    }
}
