﻿# 🕊️ Escape from Pigeonia 🕊️

## 🎮 游戏简介

在神秘的鸽子国度"Pigeonia"中，玩家们不可解释地被传送到了一个阴森的地牢里，赋予了他们神秘的力量。这个迷宫充满了敌意的生物，它们决心阻止你的每一步。接受挑战，利用你新发现的能力，穿越险恶的路径，成为这个充满无情对手的领域的胜者。

## 🗺️ 地图生成

地图覆盖一个23x23的网格，内含两种障碍物——15个可破坏的障碍物（每个有3点生命值），以及15个不可破坏的障碍物。最外层形成了一个不可穿透的边界，由不可破坏的障碍物构成，为玩家的生存挑战创建了一个封闭的竞技场。

## 🦸‍♂️ 英雄设置

- 生命值：30颗心（进入第二张地图时恢复至30颗心）
- 速度：每秒2.5单位
- 近战攻击：2颗心的伤害，2秒的冷却时间，3x3单位的范围效果
- 远程攻击：1颗心的伤害，0.5秒的冷却时间，子弹速度为每秒2单位，单一目标效果

## 🎮 控制方式

- 左键点击：远程攻击
- 右键点击：近战攻击
- WASD：四个方向的移动
- 空格键：暂停/恢复游戏

## 👹 敌人

- 近战敌人：2颗心的生命值，1颗心的伤害，3x3单位的范围，1秒的冷却时间
- 远程敌人：1颗心的生命值，1颗心的伤害，无限范围，1秒的冷却时间，子弹速度为每秒2单位

## 🔄 敌人生成

- 第一张地图：10个近战敌人
- 第二张地图：15个近战和5个远程敌人
  敌人在近战敌人被杀后2秒内、远程敌人被杀后4秒内在随机位置重生。

## ✨ 实现的特点及加分项

- 基础寻路和障碍物避免：近战和远程敌人都使用AI在地形中导航，避免障碍物，并与玩家相对位置策略性定位。远程敌人在英雄靠近时会进行近战，展示了高级战术动作。
- 动态敌人行为：近战敌人进行近距离战斗，而远程敌人保持最优距离。远程敌人表现出智能射击对准，总是朝向英雄，增加了游戏的挑战性。
- 子弹动态：子弹在撞击时消失，并不穿过障碍物，确保了公平的游戏玩法。
- 增强的UI和HUD：包括倒计时计时器、分数跟踪、生命状态可视化和攻击冷却.

<br>

## ✨ 图片预览

- <figure>
  <img src="Img/6.png" alt="图片描述">
  <figcaption>主界面</figcaption>
</figure>

- <figure>
  <img src="Img/7.png" alt="图片描述">
  <figcaption>关于界面</figcaption>
</figure>

- <figure>
  <img src="Img/8.png" alt="图片描述">
  <figcaption>排名界面</figcaption>
</figure>

- <figure>
  <img src="Img/1.png" alt="图片描述">
  <figcaption>游戏第一关</figcaption>
</figure>

- <figure>
  <img src="Img/2.png" alt="图片描述">
  <figcaption>近战攻击可破坏障碍物</figcaption>
</figure>

- <figure>
  <img src="Img/3.png" alt="图片描述">
  <figcaption>遭到攻击</figcaption>
</figure>

- <figure>
  <img src="Img/4.png" alt="图片描述">
  <figcaption>敌人和英雄的远程攻击</figcaption>
</figure>

- <figure>
  <img src="Img/5.png" alt="图片描述">
  <figcaption>暂停,失败和胜利</figcaption>
</figure>



