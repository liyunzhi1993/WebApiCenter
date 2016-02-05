### 1、技术架构

架构:asp.net mvc restful web api

前端框架:bootstrap

数据库:mongodb

#### 2、数据库设计

表名：ApiLibrary Api库

ID ObjectId 主键

apiname string api名称

description api描述

docurl 接口文档地址

表名：Authorizations 权限授权表

ID ObjectId 主键

Apilibraryids api库id集合 用,分割

Clientsid 客户端ID

表名：Clients 客户端表

ID ObjectId 主键

Clientname 客户端名称

Clientid api_key

Clientsecret api_secret

Reqip 限制访问IP

Createtime 创建时间

Isenabled 状态(正常/暂停)

表名：Users 用户表

Account 账号

Password 密码

#### 3、项目代码结构

![](http://upload-images.jianshu.io/upload_images/1556465-846cb5cd052d36e6.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)


#### 4、控制中心页面

![](http://upload-images.jianshu.io/upload_images/1556465-59da79f9380ed149.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)


#### 5、接口API规范

**接口地址（正式环境）：**http://正式地址/api

调用方式：Get或Post皆可

**接口参数：**

![](http://upload-images.jianshu.io/upload_images/1556465-13b4d3cce35cd217.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)


返回代码：

![](http://upload-images.jianshu.io/upload_images/1556465-dc2cf758ccc5559c.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)


#### 6、接口调用示例

本例将以获取积分余额功能为例,讲解如何通过服务调用相应的数据,首先我们会提供给接口调用方,两个key文件,一个APIKey和一个SecretKey,还有一个apiid如本例中的

APIKEY:CEE4975F-8E59-4FB6-A1DD-D02EDA829342

SecretKey:3C3219B7CFEB89418259CB445AC33051

APIId:56b08a110ad8f94c50e6bf05

1、提供接口方法所需的特定参数（必填）：

在Score.QueryScoreByMembercode这个积分查询接口方法中,membercode入参是必须的,在本例中分别对应138888888888,通过key=value的方式拼接参数字符串为:

method= Score.QueryScoreByMembercode&amp;membercode=138888888888

2、加上其余可选参数（可根据需要选填）：

method=Score.QueryScoreByMembercode&amp;api_key=CEE4975F-8E59-4FB6-A1DD-D02EDA829342&amp;membercode=138888888888

4、根据上面的字符串,加上secretkey值,用sh1算法加密得到sig的值

method=Score.Score.QueryScoreByMembercode&amp;api_key=CEE4975F-8E59-4FB6-A1DD-D02EDA829342&amp;membercode=138888888888

&amp;api_id=56b08a110ad8f94c50e6bf05&amp;3C3219B7CFEB89418259CB445AC33051

通过sha1算法加密上面的字符串，可以得到例如

sig=062A3A36C664FEFE0FD1472A7782969DF1AB0CF0

5、现在就可以通过完整的url,获取所需要的数据了：

www.正式地址/api?method=Score.QueryScoreByMembercode&amp;api_key=CEE4975F-8E59-4FB6-A1DD-D02EDA829342&amp;sig=062A3A36C664FEFE0FD1472A7782969DF1AB0CF0&amp;membercode=13901648617&amp;api_id=56b08a110ad8f94c50e6bf05

6、服务器返回的结果默认是Json,也可以是XML

#### 7、QA

1、登录用户怎么增加

进入发布服务器下，在C:\Program Files (x86)\MongoDB 2.6 Standard\bin打开DOS命令窗口

输入mongod &ndash;dbpath &ldquo;D:/Mongodb/DB&rdquo;

然后重新打开新的dos命令窗口

输入mongo

然后输入use WebApiCenter（WebApiCenter即数据库）

然后输入db.Users.insert({account:&rdquo;用户名&rdquo;,password:&rdquo;密码&rdquo;})

即可

2、如何增加API类库

在APILirbry下增加类库

以及在mongodb中增加

db.ApiLibrary.insert({apiname:"MobileServices",description:"专为移动端开发的API",docid:""})

项目开源地址：https://github.com/liyunzhi1993/Asp.net-Mvc-Rest-WebApiCenter
