using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    #region 定义相关变量
    public enum PieceType//网格中格子的类型,以类型为键值从字典中获取对象
    {
        Empty,//空
        Normal,//正常块
        Bubble,//障碍物
        Row_Clear,//消除四个
        Column_Clear,//消除五个
        Rainbow_Clear,
        Count,
    };
    public enum StateSprite
    {
        毒剑,毒法,毒盾,毒血,火剑, 火法, 火盾, 火血
    }
    //添加struct进行排列
    [System.Serializable]//自定义结构,在监视面板可见
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    };
    [System.Serializable]
    public struct PiecePosition
    {
        public PieceType type;
        public int x;
        public int y;
    }

    [System.Serializable]//存储不同状态图片
    public struct StateSpriteSave//存储状态图片
    {
        public StateSprite state;
        public Sprite sprite;
    }

    public StateSpriteSave[] stateSpriteSave;//存储毒和燃烧对应的状态图片

    public PiecePosition[] initialPieces;//可以设置哪些元素在哪些地方生成
    //网格行列数
    public int xDim;
    public int yDim;
    public float fillTime;//在协程中控制时间（下落动画）
    //定义字典，键是类型，值是游戏对象
    Dictionary<PieceType, GameObject> piecePrefabDict;
    //添加阵列并保存
    public PiecePrefab[] piecePrefabs;
    GamePiece[,] pieces;//保存各格子的物体上的GamePiece脚本
    public Level level;

    public GameObject BackGroundPrefab;//背景格子
    bool inverse = false;//判断物体是否可以斜着走

    bool gameOver = false;//这两个bool均控制不能移动格子
    bool isPause = false;
    GamePiece pressPiece;//鼠标按下的物体元素
    GamePiece enterPiece;//悬停的元素(松开鼠标时鼠标下的对象)
    

    #endregion

    void Awake()
    {      
        xDim = 7;
        yDim = 6;//设置生成8x7网格
        piecePrefabDict = new Dictionary<PieceType, GameObject>();

        //保存生成的格子
        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))//如过不存在该键值，则创建
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        for (int x = 0; x < xDim; x++)//在所有位置生成格子背景
        {
            for (int y = 0; y < yDim; y++)
            {
                GameObject background = (GameObject)Instantiate(BackGroundPrefab, GetWorldPosition(x, y), transform.rotation);
                background.name = "Bg(" + x + "," + y + ")";
                background.transform.parent = transform;
            }
        }

        pieces = new GamePiece[xDim, yDim];//保存物品的脚本

        for (int i = 0; i < initialPieces.Length; i++)//使用该列表设置障碍物，可在游戏界面设置
        {
            if (initialPieces[i].x >= 0 && initialPieces[i].x < xDim && initialPieces[i].y >= 0 && initialPieces[i].y < yDim)
            {
                SpawnNewPiece(initialPieces[i].x, initialPieces[i].y, initialPieces[i].type);
            }
        }

        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (pieces[x, y] == null)
                {
                    SpawnNewPiece(x, y, PieceType.Empty);
                }
            }
        }
        //SetBubble(3, 2);
        //SetBubble(3, 2);
        //SetBubble(2, 3);//设置范围为（（0~~x-1）,(0~~y-1)）
        //SetBubble(1, 4);
        //SetBubble(5, 4);
        //SetBubble(4, 4);
        StartCoroutine(Fill());//填充所有格子
    }

    //public void SetBubble(int x, int y)//在指定位置设置障碍物
    //{
    //    Destroy(pieces[x, y].gameObject);
    //    SpawnNewPiece(x, y, PieceType.Bubble);//此处设置障碍物
    //}
    #region 填充empty方法，在awake中执行
    int First = 1;
    public IEnumerator Fill()//填充所有网格 
    {

        bool needsRefill = true;//决定是否再次填充

        while (needsRefill)//一直循环创建直到没有配对的元素
        {
            yield return new WaitForSeconds(0.7f);
            while (FillStep())//控制物体移动
            {
                inverse = !inverse;
                yield return new WaitForSeconds(fillTime);
            }
            if (First == 1) { needsRefill = false; }
            else { needsRefill = ClearAllValidMatches(); }
        }
        if (First != 1)
        {
            NormalPiece nor = ClearMgr.Instance.Out();//////在此刻向外部发送消息         
            FightMgr.Instance.Init(nor);//注意，数组为引用类型，传递时不能清除
            ClearMgr.Instance.clear();//清空NormalPiece当前记录的消除数据
        }
        else { First++; }
    }
    public bool FillStep()//一步步移动，若有元素被移除则为真
    {
        bool movedPiece = false;
        for (int y = yDim - 2; y >= 0; y--)//最上面一排不用考虑，最上面一排生成物体向下移动
        {
            for (int loopX = 0; loopX < xDim; loopX++)
            {
                int x = loopX;
                if (inverse)
                {
                    x = xDim - 1 - loopX;
                }
                GamePiece piece = pieces[x, y];
                if (piece.IsMovable())//遍历所有元素，检查它是否能移动
                {
                    // print("能移动");
                    GamePiece pieceBelow = pieces[x, y + 1];
                    if (pieceBelow.Type == PieceType.Empty)//检查下面的是否为空白元素，若是空白则将上面的向下移动
                    {//直着向下移动
                        Destroy(pieceBelow.gameObject);
                        //print("删除空格子");
                        piece.MovableComponent.Move(x, y + 1, fillTime);//当前元素向下移动一格
                        pieces[x, y + 1] = piece;
                        SpawnNewPiece(x, y, PieceType.Empty);//当前元素的格子变为空
                        movedPiece = true;
                    }
                    else//下边不为空但可移动的情况
                    {
                        for (int diag = -1; diag <= 1; diag++)//diag为-1向左下角走，为1向右下角走
                        {
                            if (diag != 0)//元素在正下方
                            {
                                int diagX = x + diag;
                                if (inverse)
                                {
                                    diagX = x - diag;
                                }
                                if (diagX >= 0 && diagX < xDim)
                                {
                                    GamePiece diagonalPiece = pieces[diagX, y + 1];//当前元素下面的元素
                                    if (diagonalPiece.Type == PieceType.Empty)//若当前元素下面元素为空白
                                    {
                                        bool hasPieceAbove = true;
                                        for (int aboveY = y; aboveY >= 0; aboveY--)
                                        {
                                            GamePiece pieceAbove = pieces[diagX, aboveY];
                                            if (pieceAbove.IsMovable())//保证垂直下落
                                            {
                                                break;
                                            }
                                            else if (!pieceAbove.IsMovable() && pieceAbove.Type != PieceType.Empty)
                                            {//如果元素不可移动也不是空白的（遮挡物）
                                                hasPieceAbove = false;
                                                break;
                                            }
                                        }
                                        if (!hasPieceAbove)//无法垂直填充时，使用斜着填充
                                        {
                                            //print("斜着填充");
                                            Destroy(diagonalPiece.gameObject);
                                            piece.MovableComponent.Move(diagX, y + 1, fillTime);
                                            pieces[diagX, y + 1] = piece;
                                            SpawnNewPiece(x, y, PieceType.Empty);
                                            movedPiece = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int x = 0; x < xDim; x++)
        {
            GamePiece pieceBelow = pieces[x, 0];//顶行
            if (pieceBelow.Type == PieceType.Empty)
            {   //添加-1行参数

                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.Normal], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                pieces[x, 0].Init(x, -1, this, PieceType.Normal);
                pieces[x, 0].MovableComponent.Move(x, 0, fillTime);   //将-1行元素移动到第0行，并设置颜色（图像）随机
                pieces[x, 0].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, pieces[x, 0].ColorComponent.NumColors));
                movedPiece = true;
            }
        }
        return movedPiece;
    }
    #endregion
    public Vector2 GetWorldPosition(int x, int y)//返回存有偏移量的坐标，将整体格子居中
    {
        return new Vector2(transform.position.x - xDim / 2.0f + x, transform.position.y + yDim / 2.0f - y) * 0.7f;
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceType type)//创建各种类型的元素（障碍物，空，正常元素）
    {//创建元素
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;
        
        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        //if (pieces[x,y].type == PieceType.Bubble)//如果为障碍物，将状态变为障碍物
        //{
        //    pieces[x, y].state = State.buddle;
        //}
        pieces[x, y].Init(x, y, this, type);
        return pieces[x, y];
    }

  
    public bool IsAdjacent(GamePiece piece1, GamePiece piece2)//判断这两个格子是否相邻，若相邻则为返回true    
    {   //x相同时，y轴相差1，或y相同时，x轴相差1
        // print("(" + piece1.X + "," + piece1.Y + ")" + "(" + piece2.X + "," + piece2.Y + ")");//打印两者坐标
        return (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Y - piece2.Y) == 1)
         || (piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1);
    }

    public void SwapPieces(GamePiece piece1, GamePiece piece2)//符合条件后交换两物体位置
    {
        if (gameOver) { return; }
        if (isPause) { return; }
        // print(pieces[piece1.X, piece1.Y].X + " " + pieces[piece1.X, piece1.Y].Y +"/n"+pieces[piece2.X, piece2.Y].X + " " + pieces[piece2.X, piece2.Y].Y);

        if (piece1.IsMovable() && piece2.IsMovable())//如果两者均可移动
        {
            Pause();//暂时关闭该方法；
            pieces[piece1.X, piece1.Y] = piece2;//交换两者位置
            pieces[piece2.X, piece2.Y] = piece1;

            if (GetMatch(piece1, piece2.X, piece2.Y) != null || GetMatch(piece2, piece1.X, piece1.Y) != null
             || piece1.Type == PieceType.Rainbow_Clear || piece2.Type == PieceType.Rainbow_Clear)
            //若没有返回空白，则为匹配,若为Rainbow，则可直接与任意元素匹配
            {
                int piece1X = piece1.X;
                int piece1Y = piece1.Y;//暂存piece1的坐标

                piece1.MovableComponent.Move(piece2.X, piece2.Y, fillTime);//互换时的动作
                piece2.MovableComponent.Move(piece1X, piece1Y, fillTime);

                //若为五连消生成的物体，则执行该操作进行消除
                if (piece1.Type == PieceType.Rainbow_Clear && piece1.IsClearable() && piece2.IsColored())
                {
                    ClearColorPiece clearColor = piece1.GetComponent<ClearColorPiece>();
                    if (clearColor)
                    {
                        piece1.MaxSkill = piece2.nowColor();//保存和特殊方块一起消除的颜色
                        clearColor.Color = piece2.ColorComponent.Color;
                    }
                    ClearPiece(piece1.X, piece1.Y);
                }
                if (piece2.Type == PieceType.Rainbow_Clear && piece2.IsClearable() && piece1.IsColored())
                {
                    ClearColorPiece clearColor = piece2.GetComponent<ClearColorPiece>();
                    if (clearColor)
                    {
                        piece2.MaxSkill = piece1.nowColor();//保存和特殊方块一起消除的颜色
                        clearColor.Color = piece1.ColorComponent.Color;
                    }
                    ClearPiece(piece2.X, piece2.Y);
                }
                //检查piece1和piece2的type是否是特殊图标,若是则清除
                if (piece1.Type == PieceType.Rainbow_Clear || piece2.Type == PieceType.Rainbow_Clear)
                {
                    ClearPiece(piece1.X, piece1.Y);
                    ClearPiece(piece2.X, piece2.Y);
                }
                ClearAllValidMatches(); // 调用清除方法

                pressPiece = null;
                enterPiece = null;//当拖动后清除完，将拖动对象赋为空，在clearall方法中检索，判断是否生成特殊元素

                StartCoroutine(Fill());//清楚后补充
                level.OnMove();
                //关卡脚本判断人物移动的方法
            }
            else//如果移动后没有匹配的三个，则交换两者
            {

                Debug.Log("移不动");
                Start();
                pieces[piece1.X, piece1.Y] = piece1;//交换两者位置
                pieces[piece2.X, piece2.Y] = piece2;
                //piece1.MovableComponent.Move(piece2.X, piece2.Y, fillTime);//互换时的动作
                //piece2.MovableComponent.Move(piece1X, piece1Y, fillTime);
            }
            //  print(pieces[piece1.X, piece1.Y].X + " " + pieces[piece1.X, piece1.Y].Y + "/n" + pieces[piece2.X, piece2.Y].X + " " + pieces[piece2.X, piece2.Y].Y);
        }
    }
    #region 鼠标监听控制方块互换位置，调用交换方法
    public void PressPiece(GamePiece piece)//监听鼠标的进入功能
    {
        pressPiece = piece;
    }
    public void EnterPiece(GamePiece piece)//监听鼠标悬停功能
    {
        enterPiece = piece;
    }
    public void ReleasePiece()
    {
        if (IsAdjacent(pressPiece, enterPiece))//检测这两个元素是否相邻
        {
            //print("相邻");
            SwapPieces(pressPiece, enterPiece);//互换
        }
        else
        { //print("不相邻"); 
        }
    }
    #endregion

    public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY)//获取可以配对的元素
    {
        if (piece.IsColored())//如果存在颜色（图片）
        {
            ColorPiece.ColorType color = piece.ColorComponent.Color;
            List<GamePiece> horizontalPieces = new List<GamePiece>();
            List<GamePiece> verticalPieces = new List<GamePiece>();//检查当前物体的左右上下格子如果颜色相同保存在链表中
            List<GamePiece> matchingPieces = new List<GamePiece>();

            horizontalPieces.Add(piece);//把当前格子加入水平链表
            for (int dir = 0; dir <= 1; dir++)//改变元素走向，0和1代表不同方向
            {
                for (int xOffSet = 1; xOffSet < xDim; xOffSet++)//遍历两者相邻元素
                {
                    int x;
                    if (dir == 0)//dir=0时代表左边，1时为右边
                    {
                        x = newX - xOffSet;//左边的x坐标
                    }
                    else
                    {
                        x = newX + xOffSet;
                    }
                    if (x < 0 || x >= xDim)//超出边界时break
                    {
                        break;
                    }
                    if (pieces[x, newY].IsColored() && pieces[x, newY].ColorComponent.Color == color)
                    {
                        //先查看水平列，故y轴不变,判断该格子是否存在颜色，且和本格子颜色是否相同，匹配则保存入水平链表
                        horizontalPieces.Add(pieces[x, newY]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (horizontalPieces.Count >= 3)//如果水平相同颜色存在3个，则放入match匹配链表
            {
                for (int i = 0; i < horizontalPieces.Count; i++)
                {
                    matchingPieces.Add(horizontalPieces[i]);
                }   
            }
            if (horizontalPieces.Count >= 3)//L型匹配
            {
                for (int i = 0; i < horizontalPieces.Count; i++)//当存在可配对时，遍历所有元素
                {
                    for (int dir = 0; dir <= 1; dir++)//0上1下 通过水平检查竖直的情况
                    {
                        for (int yOffset = 1; yOffset < yDim; yOffset++)
                        {
                            int y;
                            if (dir == 0)
                            {
                                y = newY - yOffset;
                            }
                            else
                            {
                                y = newY + yOffset;
                            }
                            if (y < 0 || y >= yDim)//超越边界就停止
                            {
                                break;
                            }
                            if (pieces[horizontalPieces[i].X, y].IsColored() && pieces[horizontalPieces[i].X, y].ColorComponent.Color == color)
                            {
                                //检测自己的竖直方向是否存在相同物品，若存在则保存到vertical列表中
                                verticalPieces.Add(pieces[horizontalPieces[i].X, y]);
                            }
                            else
                            {
                                break;//不匹配则停止 
                            }
                        }
                    }
                    //检测vectorial链表中的数量
                    if (verticalPieces.Count < 2)//除去公用的，只有两个相邻则达标，负责清空继续循环
                    {
                        verticalPieces.Clear();
                    }
                    else
                    {  //如果大于2，则加入matching匹配链表
                        for (int j = 0; j < verticalPieces.Count; j++)
                        {
                            matchingPieces.Add(verticalPieces[j]);
                        }
                        break;
                    }
                }


            }

            if (matchingPieces.Count >= 3)//存入元素大于3个，则返回
            {
                return matchingPieces;
            }

            //垂直列表
            horizontalPieces.Clear();
            verticalPieces.Clear();//上面使用后清空链表

            verticalPieces.Add(piece);//把当前格子加入水平链表
            for (int dir = 0; dir <= 1; dir++)//改变元素走向，0和1代表不同方向
            {
                for (int yOffSet = 1; yOffSet < yDim; yOffSet++)//遍历两者相邻元素
                {
                    int y;
                    if (dir == 0)//dir=0时代表上，1时为下
                    {
                        y = newY - yOffSet;//左边的x坐标
                    }
                    else
                    {
                        y = newY + yOffSet;
                    }
                    if (y < 0 || y >= yDim)//超出边界时break
                    {
                        break;
                    }
                    if (pieces[newX, y].IsColored() && pieces[newX, y].ColorComponent.Color == color)
                    {
                        //先查看水平列，故y轴不变,判断该格子是否存在颜色，且和本格子颜色是否相同，匹配则保存入水平链表
                        verticalPieces.Add(pieces[newX, y]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (verticalPieces.Count >= 3)//如果水平相同颜色存在3个，则放入match链表
            {
                for (int i = 0; i < verticalPieces.Count; i++)
                {
                    matchingPieces.Add(verticalPieces[i]);
                }
            }

            if (verticalPieces.Count >= 3)//L型匹配
            {
                for (int i = 0; i < verticalPieces.Count; i++)//当存在可配对时，遍历所有元素
                {
                    for (int dir = 0; dir <= 1; dir++)//0左1右 通过竖直检查水平的情况
                    {
                        for (int xOffset = 1; xOffset < xDim; xOffset++)
                        {
                            int x;
                            if (dir == 0)
                            {
                                x = newX - xOffset;
                            }
                            else
                            {
                                x = newX + xOffset;
                            }
                            if (x < 0 || x >= xDim)//超越边界就停止
                            {
                                break;
                            }
                            if (pieces[x, verticalPieces[i].Y].IsColored() && pieces[x, verticalPieces[i].Y].ColorComponent.Color == color)
                            {
                                //检测自己的水平方向是否存在相同物品，若存在则保存到vertical列表中
                                horizontalPieces.Add(pieces[x, verticalPieces[i].Y]);
                            }
                            else
                            {
                                break;//不匹配则停止 
                            }
                        }
                    }
                    //检测horizontalPieces链表中的数量
                    if (horizontalPieces.Count < 2)//除去公用的，只有两个相邻则达标，负责清空继续循环
                    {
                        horizontalPieces.Clear();
                    }
                    else
                    {  //如果大于2，则加入matching匹配链表
                        for (int j = 0; j < horizontalPieces.Count; j++)
                        {
                            matchingPieces.Add(horizontalPieces[j]);
                        }
                        break;
                    }
                }
            }

            if (matchingPieces.Count >= 3)//符合标准则返回匹配(至少三个元素)
            {              
                return matchingPieces;
            }
        }
        return null;
    }

    public bool ClearAllValidMatches()//遍历所有网格清除,一次消除多个生成特殊方块
    {
        bool needsRefill = false;
        for (int y = 0; y < yDim; y++)
        {
            for (int x = 0; x < xDim; x++)
            {
                if (pieces[x, y].IsClearable())//若元素可以清除，检查配对
                {
                    List<GamePiece> match = GetMatch(pieces[x, y], x, y);
                    if (match != null)//如果配对，则检查全部,并清除该配对
                    {
                        ///  saveClear(match);//////////////////遍历所有元素，并存入需要传出的数据
                        PieceType specialPieceType = PieceType.Count;
                        GamePiece randomPiece = match[Random.Range(0, match.Count)];
                        int specialPieceX = randomPiece.X;
                        int specialPieceY = randomPiece.Y;//随机生成位置
                        if (match.Count == 4)//如果匹配的元素为四个
                        {
                            //若这两者均为null，则证明不是因为互换元素引起的
                            specialPieceType = PieceType.Row_Clear;
                            //生成小技能图标
                        }
                        else if (match.Count == 5)
                        {
                            specialPieceType = PieceType.Rainbow_Clear;//可消除所有同图片的物体
                        }

                        //if (match.Count == 5)//如果匹配的元素为五个
                        //{                           
                        //    specialPieceType = PieceType.Column_Clear;//生成大技能图标
                        //}

                        for (int i = 0; i < match.Count; i++)
                        {
                            if (ClearPiece(match[i].X, match[i].Y))
                            {
                                needsRefill = true;//清除后需要填充
                                if (match[i] == pressPiece || match[i] == enterPiece)//如果是在交换后生成的
                                {
                                    specialPieceX = match[i].X;
                                    specialPieceY = match[i].Y;
                                }
                            }
                        }

                        if (specialPieceType != PieceType.Count)//判断是否生成特殊图标
                        {
                            // print(123);
                            Destroy(pieces[specialPieceX, specialPieceY]);
                            GamePiece newPiece = SpawnNewPiece(specialPieceX, specialPieceY, specialPieceType);
                            if (specialPieceType == PieceType.Row_Clear && newPiece.IsColored() && match[0].IsColored())
                            {//当为四连消时， 生成特殊图标并赋给被消除的物品图片  //在swappieces中设置额外的
                                newPiece.ColorComponent.SetColor(match[0].ColorComponent.Color);
                            }
                            //五连消生成
                            else if (specialPieceType == PieceType.Rainbow_Clear && newPiece.IsColored())
                            {
                                newPiece.ColorComponent.SetColor(ColorPiece.ColorType.ANY);
                            }
                        }
                    }
                }
            }
        }

        return needsRefill;
    }

    public bool ClearPiece(int x, int y)//清除单个元素的方法
    {
        if (pieces[x, y].IsClearable() && !pieces[x, y].ClearableComponent.IsBeingCleared)
        {    //判断该元素是否可以被清除，若可以清除，则新建空白元素代替它
            pieces[x, y].ClearableComponent.Clear();
            SpawnNewPiece(x, y, PieceType.Empty);
            ClearObstacles(x, y);//查找附近障碍物并删除障碍物
            return true;
        }
        return false;
    }

    public void ClearObstacles(int x, int y)//当传入元素坐标时，搜索该元素附近的阻碍物并将之删除(在上方法中调用)
    {
        for (int adjacentX = x - 1; adjacentX <= x + 1; adjacentX++)
        {
            if (adjacentX != x && adjacentX >= 0 && adjacentX < xDim)
            {//检查左右是否存在障碍物，若存在则销毁障碍物，同时创建一个empty在其上面
                if (pieces[adjacentX, y].Type == PieceType.Bubble && pieces[adjacentX, y].IsClearable())
                {
                    pieces[adjacentX, y].ClearableComponent.Clear();
                    SpawnNewPiece(adjacentX, y, PieceType.Empty);
                }
            }
        }
        for (int adjacentY = y - 1; adjacentY <= y + 1; adjacentY++)//检查上下是否存在元素
        {
            if (adjacentY != y && adjacentY >= 0 && adjacentY < yDim)
            {
                if (pieces[x, adjacentY].Type == PieceType.Bubble && pieces[x, adjacentY].IsClearable())
                {
                    pieces[x, adjacentY].ClearableComponent.Clear();
                    SpawnNewPiece(x, adjacentY, PieceType.Empty);
                }
            }
        }
    }

    public void ClearRow(int row)//消除一列元素
    {
        for (int x = 0; x < xDim; x++)
        {
            ClearPiece(x, row);
        }
    }
    public void ClearColumn(int column)//清除一行元素
    {
        for (int y = 0; y < yDim; y++)
        {
            ClearPiece(column, y);
        }
    }
    public void ClearColor(ColorPiece.ColorType color) //查找所有的同色物体将之清除,当两个物体均为any时，全部清除
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (pieces[x, y].IsColored() && (pieces[x, y].ColorComponent.Color == color || color == ColorPiece.ColorType.ANY))
                {
                    ClearPiece(x, y);
                }
            }
        }
    }

    public void Start()
    {
        // Debug.Log("开始");
        isPause = false;
    }
    public void Pause()
    {
        // Debug.Log("暂停");
        isPause = true;
    }
    public void GameOver()
    {
        gameOver = true;
    }

    public List<GamePiece> GetPiecesOfType(PieceType type)//传入元素类型，传出所有同类型物品
    {
        List<GamePiece> piecesOfType = new List<GamePiece>();
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (pieces[x, y].Type == type)
                {
                    piecesOfType.Add(pieces[x, y]);
                }
            }
        }
        return piecesOfType;
    }


    #region 结合技能系统模块
    public bool InstanceBubble(int x, int y)//清除单个元素并将之替换为障碍物
    {
        if (pieces[x, y].IsClearable() && !pieces[x, y].ClearableComponent.IsBeingCleared && pieces[x,y].state == State.normal)
        {    //判断该元素是否可以被清除，若可以清除，则新建空白元素代替它
            pieces[x, y].ClearableComponent.Clear();
            SpawnNewPiece(x, y, PieceType.Bubble);
            ChangeState(x, y, State.buddle);//修改状态为障碍物
            //ClearObstacles(x, y);//查找附近障碍物并删除障碍物
            return true;
        }
        return false;
    }
    public bool ChangeState(int x, int y, State state)//给单个元素添加状态
    {
        if (pieces[x, y].state == State.normal && pieces[x, y].Type == PieceType.Normal)
        {
            pieces[x, y].state = state;
            if (pieces[x, y].state == State.burn)
            {
                switch (pieces[x,y].nowColor())
                {
                    case ColorPiece.ColorType.SWORD:
                        pieces[x, y].ChangeSprite(stateSpriteSave[4].sprite);
                        break;
                    case ColorPiece.ColorType.SHIELD:
                        pieces[x, y].ChangeSprite(stateSpriteSave[6].sprite);
                        break;
                    case ColorPiece.ColorType.MAGIC:
                        pieces[x, y].ChangeSprite(stateSpriteSave[5].sprite);
                        break;
                    case ColorPiece.ColorType.HEART:
                        pieces[x, y].ChangeSprite(stateSpriteSave[7].sprite);
                        break;                  
                    default:
                        break;
                }
                return true;
            }
            if (pieces[x, y].state == State.poison)
            {
                switch (pieces[x, y].nowColor())
                {
                    case ColorPiece.ColorType.SWORD:
                        pieces[x, y].ChangeSprite(stateSpriteSave[0].sprite);
                        break;
                    case ColorPiece.ColorType.SHIELD:
                        pieces[x, y].ChangeSprite(stateSpriteSave[2].sprite);
                        break;
                    case ColorPiece.ColorType.MAGIC:
                        pieces[x, y].ChangeSprite(stateSpriteSave[1].sprite);
                        break;
                    case ColorPiece.ColorType.HEART:
                        pieces[x, y].ChangeSprite(stateSpriteSave[3].sprite);
                        break;
                    default:
                        break;
                }
                return true;
            }
        }
        return false;
    }
    public int[] CheckState()//查找场中存在有状态的元素个数并返回
    {
        int burn = 0;
        int poision = 0;
        int buddle = 0;
        for (int y = 0; y < yDim; y++)
        {
            for (int x = 0; x < xDim; x++)
            {
                if (pieces[x, y].state == State.burn)
                {
                    burn++;//记录场中元素存在多少个燃烧状态
                }
                if (pieces[x, y].state == State.poison)
                {
                    poision++;
                }
                if (pieces[x, y].state == State.buddle)
                {
                    buddle++;
                }
            }
        }
        int[] i = { burn,poision,buddle};
        return i;
    }
    public void HuifuState(int num=1)
    {
        int i = 0;
        for (int y = 0; y < yDim; y++)
        {
            if (i == num) { break; }
            for (int x = 0; x < xDim; x++)
            {
                if (pieces[x, y].state == State.burn)
                {
                    pieces[x, y].state = State.normal;
                    pieces[x, y].gameObject.GetComponent<ColorPiece>().Color = pieces[x, y].nowColor(); 
                    i++;
                }
                if (pieces[x, y].state == State.poison)
                {
                    pieces[x, y].state = State.normal;
                    pieces[x, y].gameObject.GetComponent<ColorPiece>().Color = pieces[x, y].nowColor();
                    i++;
                }
                if (i == num) { break; }
            }
        }
    }
    #endregion
    // public List<NormalPiece> ClearEven = new List<NormalPiece>();//外部接口  保存和向外界返回本次移动后消除的对象类型
}
