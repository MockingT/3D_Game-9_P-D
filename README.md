# 3D_Game-9_P-D  
视频链接：http://www.iqiyi.com/w_19ryzvh9jh.html  
### 大致效果  
![avatar](https://github.com/MockingT/3D_Game-9_P-D/blob/master/pictures/2.png)  
- 状态图采用了去年师兄的状态图  
![avatar](https://github.com/MockingT/3D_Game-9_P-D/blob/master/pictures/1.png)  
- 根据状态图，借鉴了师兄的博客，每一次点击next都会分析当前的状态，以及判断左右牧师和魔鬼的个数。然后用if-else对其判断决定下一步该如何走。同样也使用了随机函数去决定当有多条路径选择时使用哪一条。因为不想再添加新的枚举变量去标注当前游戏的状态，所以直接在判断下面调用priestStartOnBoat();devilStartOnBoat();boat_move()等函数，省去了一些步骤。之后只需要再更新IUserActions接口，在界面中添加一个button即可。  
![avatar](https://github.com/MockingT/3D_Game-9_P-D/blob/master/pictures/3.png)  
- 代码部分见https://github.com/MockingT/3D_Game-9_P-D/tree/master/Assets/script
