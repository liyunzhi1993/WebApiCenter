# Asp.net-Mvc-Rest-WebApiCenter

1、技术架构
架构:asp.net mvc restful web api

前端框架:bootstrap

数据库:mongodb

2、数据库设计
表名：ApiLibrary Api库

ID	ObjectId 主键

apiname string api名称

description api描述

docurl 接口文档地址

表名：Authorizations 权限授权表

ID	ObjectId 主键

Apilibraryids api库id集合 用,分割

Clientsid 客户端ID

表名：Clients 客户端表

ID	ObjectId 主键

Clientname 客户端名称

Clientid api_key

Clientsecret api_secret

Reqip 限制访问IP

Createtime 创建时间

Isenabled 状态(正常/暂停)

表名：Users 用户表

Account 账号

Password 密码

3、接口调用示例
本例将以获取积分余额功能为例,讲解如何通过服务调用相应的数据,首先我们会提供给接口调用方,两个key文件,一个APIKey和一个SecretKey,还有一个apiid如本例中的

APIKEY:CEE4975F-8E59-4FB6-A1DD-D02EDA829342

SecretKey:3C3219B7CFEB89418259CB445AC33051

APIId:56b08a110ad8f94c50e6bf05

1、提供接口方法所需的特定参数（必填）：

在Score.QueryScoreByMembercode这个积分查询接口方法中,membercode入参是必须的,在本例中分别对应138888888888,通过key=value的方式拼接参数字符串为:

method= Score.QueryScoreByMembercode&membercode=138888888888

2、加上其余可选参数（可根据需要选填）：

method=Score.QueryScoreByMembercode&api_key=CEE4975F-8E59-4FB6-A1DD-D02EDA829342&membercode=138888888888

4、根据上面的字符串,加上secretkey值,用sh1算法加密得到sig的值

method=Score.Score.QueryScoreByMembercode&api_key=CEE4975F-8E59-4FB6-A1DD-D02EDA829342&membercode=138888888888

&api_id=56b08a110ad8f94c50e6bf05&3C3219B7CFEB89418259CB445AC33051

通过sha1算法加密上面的字符串，可以得到例如

sig=062A3A36C664FEFE0FD1472A7782969DF1AB0CF0

5、现在就可以通过完整的url,获取所需要的数据了：

正式地址/api?method=Score.QueryScoreByMembercode&api_key=CEE4975F-8E59-4FB6-A1DD-D02EDA829342&sig=062A3A36C664FEFE0FD1472A7782969DF1AB0CF0&membercode=13901648617&api_id=56b08a110ad8f94c50e6bf05

6、服务器返回的结果默认是Json,也可以是XML

4、QA
1、登录用户怎么增加

进入发布服务器下，在C:\Program Files (x86)\MongoDB 2.6 Standard\bin打开DOS命令窗口

输入mongod –dbpath “D:/Mongodb/DB”

然后重新打开新的dos命令窗口

输入mongo

然后输入use WebApiCenter（WebApiCenter即数据库）

然后输入db.Users.insert({account:”用户名”,password:”密码”})

即可

2、如何增加API类库

在APILirbry下增加类库

以及在mongodb中增加

db.ApiLibrary.insert({apiname:"MobileServices",description:"专为移动端开发的API",docid:""})

其他可查看：http://www.cnblogs.com/liyunzhi/articles/5182914.html
