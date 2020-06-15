## Instruction cost
---
> 绝大部分GPU是一次性计算4个分量，计算一个float4和只计算单个float耗时是一样的。当计算float时，剩下三个分量的时长会被浪费。[REFERENCE1]

> float4 计算视为1个op。

> tex2D操作数未知，因有io存在，视为10op。

> 以下指令分为Unity和cg指令，Unity指令根据相应公式推算出来，cg根据[实现原理]推算出来。


|instruction|opertaion code|
|:-|:-:|
UnityObjectToClipPos|4
UnityObjectToViewPos|8
UnityObjectToWorldNormal|3
ComputeScreenPos|2
COMPUTE_EYEDEPTH|8
*|1
+|0
-|0
/|1
?|1
abs|0
max|1
clamp|2
saturate|0
floor|1
ceil|1
round|3
frac|1
exp|2
log|2
log10|2
dot|1
cross|2
min|1
max|1
all|4
any|3
fmod|4
[fwidth]|4
sin|1
cos|1
tan|1
sincos|1
asin|11
acos|10
atan|16
atan2|22
pow|3
sqrt|1
rsqrt|1
sign|3
step|2
lerp|2
smoothstep|7
distance|3
length|2
normalize|3
faceforward|2
reflect|3
mul|4
transpose|4
[ddx]|1
ddy|1
tex2D|10



[REFERENCE1]:https://zhuanlan.zhihu.com/p/34629262
[实现原理]:http://developer.download.nvidia.cn/cg/acos.html
[fwidth]:http://developer.download.nvidia.cn/cg/fwidth.html
[ddx]:https://blog.csdn.net/yaorongzhen123/article/details/89483720
