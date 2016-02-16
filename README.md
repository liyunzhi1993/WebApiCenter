### 1、概述

WebAPICenter是指对外接口的统一控制中心，包括权限分配，以及未来的日志记录，可控更安全。

### 2、技术架构

架构:asp.net mvc restful web api

前端框架:bootstrap、angularjs

数据库:mongodb

#### 3、项目代码结构

![](http://upload-images.jianshu.io/upload_images/1556465-846cb5cd052d36e6.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)


#### 4、控制中心页面

![](http://upload-images.jianshu.io/upload_images/1556465-59da79f9380ed149.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)


#### 5、接口API规范

**接口地址（正式环境）：**http://正式地址/api

调用方式：Get或Post皆可

**接口参数：**

必选项	名称	类型	描述
必选	method	string	方法名称,必须是API文档中的方法名称，否则会返回错误代码 3 (未知方法或方法内部错误)
必选	api_key 	string 	API key 对应当前请求api的应用程序。
必选	sig 	string 	请求当前方法的签名. sig需要小写匹配，否则会返回一个签名错误(104)签名由下列算法构造: 
把所有参数按照参数key字母排序，然后按照key=value的方式把所有参数拼接为字符串，最后拼接密钥SecretKey的值。例如：key1=value1&key2=value2&key3=value3&secretkey，最后用标准SHA1加密。
必选	api_id	string	即要访问的API 库ID 这个会分配
可选	format	string 	所需的返回格式JSON（默认）或XML，若需要返回XML类型数据格式则需要该参数

返回代码：

代码	描述
0	成功
1	发生未知错误,请重新提交请求
2	服务现在不可用
3	未知方法
4	已到达最大请求
30	未知访问类
100	指定的参数之一丢失或无效
103	提交的call_id不大于该会话的前call_id
5	请求来自于该应用程序不允许的远程地址
6	没有对应的API
7	没有访问该API的权限
10	应用程序没有此操作权限
101	API没有与任何已知的应用程序关联的
102	Session是不正确提交或已超时,用户直接登录以获得另一个密钥
104	错误签名
109	不允许注册
110	用户名已经存在
112	不允许的用户名
113	用户已经在线

#### 6、接口调用示例

首先我们会提供给接口调用方,两个key文件,一个APIKey和一个SecretKey,还有一个apiid如本例中的

APIKEY:CEE4975F-8E59-4FB6-A1DD-D02EDA829342

SecretKey:3C3219B7CFEB89418259CB445AC33051

APIId:56b08a110ad8f94c50e6bf05

1、提供接口方法所需的特定参数（必填）：

在Score.QueryScoreByMembercode这测试接口方法中,membercode入参是必须的,在本例中分别对应138888888888,通过key=value的方式拼接参数字符串为:

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
