# My-first-Internship-task
9/30

第一天，学习unity。

10/1

完成角色敌人移动动画、逻辑、碰撞检测、武器、子弹发射。

10/2

完成靶心UI、子弹UI、粗糙技能特效及释放效果、武器动画、人物特效、生命值UI、经验条UI、经验球实体，目前未实装怪物掉落经验球和经验球本身的寻路功能、人物升级之后的功能。

10/3

优化了怪物移动逻辑，实现基础升级效果（生命值增加、攻击力增加），其中生命值增加会同步到生命值UI中。补充角色如流血、射击视角震动等特效。优化全局光照，模拟黑夜氛围，并为特殊对象添加了光照。简单做了个UI计时系统，同时为所有对象添加了音效，添加了背景音乐，并完成怪物的随机生成逻辑，但树的生成逻辑有待优化。

10/4

优化树生成逻辑，添加开始游戏界面、暂停游戏菜单、游戏结算（胜利与死亡）界面，初版Demo完成，后续准备添加穿透子弹、添加新升级功能。当前已添加武器选择UI，但新武器与穿透子弹尚未实装。为原有武器添加了新的升级功能，在每次升级时有一定概率获取同位体武器。

10/5

学习对象池并用对象池重构了目前子弹和怪物的生成，同时修复了大量从直接生成到使用对象池生成对象过渡时引发的bug，调整了些许UI。目前运行状态良好（游玩过程中未出现崩溃问题），子弹的轨迹问题和怪物受伤是变红而不是变白、HeartUI中新增Heart与原有Heart动画不一致、人物转向导致换弹节点转向等这类小bug视为基本不影响游戏，暂未制定修复计划，由于时间原因，本次memo实习任务就到今天结束。
