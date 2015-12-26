#RFID 脚本:充值和基本消费

# 充值和基本消费 #

以下脚本通过了D8U硬件测试；

```

= Details =
#充值和普通交易
#create by samonsun 2011-1-1

SET SAM0=SAM 0x0C
SET SAM1=SAM 0x0D

#----------------------密钥环境设置开始---------------
#应用主控子密钥
SET CPU_DACK=6641E72668A152FD3C9B71835AFAFC4A
#应用维护子密钥
SET CPU_DAMK=BF6F8F9EB4D6FE19A6C3FFEC27594AFF
#互通复合消费维护密钥
SET CPU_DAMK1=8CEEED04A6B4A950C7097CC96264D768

#消费子密钥
SET CPU_DPK=8CEEED04A6B4A950C7097CC96264D768
#圈存子密钥
SET CPU_DLK=6641E72668A152FD3C9B71835AFAFC4A
#TAC子密钥
SET CPU_DTK=F482976B32E387497D96E724D1F1B677

#消费密钥索引
SET CPU_DPK_IDX=01
#圈存密钥索引
SET CPU_DLK_IDX=01


#PIN
SET CPU_PIN=1122334455667788
SET CPU_PIN_IDX=00 
SET CPU_PIN_RELOAD=6641E72668A152FD3C9B71835AFAFC4A
SET CPU_PIN_UNLOCK=6641E72668A152FD3C9B71835AFAFC4A

#----------------------密钥环境设置结束---------------

SET CARD_RAND_8BYTE=00 00 00 00 00 00 00 00
SET CARD_RAND_4BYTE=00 00 00 00

#安全认证码变量
SET SACODE=00 00 00 00 00 00 00 00 00


#终端机编号
SET TERMINAL_ID=00 00 00 00 00 01
SET STATION_ID=00 00 00 8D

#----------------------预处理----------------------
REQUEST CARD
#选择主目录读发行信息
00 A4 00 00 02 3F 00

#Read Binary 0005
00 B0 85 00 1E
#检查应用序列号byte[8]--byte[15] 是否是黑名单，是黑名单则进入黑名单处理流程
BUFF 8,8,CARD_SNR

#检查返回结果的byte[10](0x01则为普通地铁储值票，0x03则为员工卡）
#根据上面的检查结果，判定是进行储值票交易还是员工卡交易

#选择储值票应用 1001
00 A4 00 00 02 10 01
#COS返回0015文件的内容
#byte[22]--byte[51] 为3F00/1001/0015文件内容
#0015文件：需检查应用有效期

#判定是否本地卡 byte[24~25]的值是否为 沈阳城市代码1161
#若是本地卡，进入本地卡消费流程0x19文件，需进行钱包零值初始化获取钱包当前余额，此操作可在应用程序启动时完成，作为系统常量存在
#若是异地卡，需判定此字段值是否系统参数【互通城市列表】中的值，是则进入异地互联互通消费流程0x17文件

#获取钱包余额 若钱包余额为0，则提示充值
00 20 00 $CPU_PIN_IDX 08 $CPU_PIN
SET CARD_BALANCE={ 80 5C 00 02 04}

#读取本地复合记录文件获取钱包透支金额
#读取记录1 
BUFF 00,{00 B2 01 CC 00}
#byte[0]为卡状态标志。 对于储值票而言，0x03表示已售；需检查此标志；
#检查透支金额 包大于0且透支金额不为0，则说明上次交易不完整，需要查明原因，并更新透支金额；钱包等于0，透支金额不为0时，只能进行充值操作。

#读取记录2 
BUFF 00,{00 B2 02 CC 00}


##############充值交易########################
00 A4 00 00 02 3F 00/1001
#必须VERIFY PIN
00 20 00 $CPU_PIN_IDX 08 $CPU_PIN
#以下流程遵循FM1208 200907文档
#PSAM卡根据应用序列号分散出圈存密钥 需查询PSAM卡相关功能????
#Init for load
BUFF,0,{80 50 00 02 0B 01 00 00 00 F0 $TERMINAL_ID 10}
BUFF 0,4,OLD_BLANCE
BUFF 4,2,CPU_TRADE_SN
BUFF 8,4,CARD_RAND_4BYTE
SET SESS_KEY={TripDES($CARD_RAND_4BYTE $CPU_TRADE_SN 80 00,$CPU_DLK)}

SET NOW={DATETIME}
BUFF,0,{KEY08MAC(00 00 00 F0 02 $TERMINAL_ID $NOW ,$SESS_KEY,0000000000000000)}
BUFF 0,4,MAC2

80 52 00 00 0B $NOW $MAC2 04

00 20 00 $CPU_PIN_IDX 08 $CPU_PIN
SET CARD_BALANCE={80 5C 00 02 04}


###############消费交易#######################
00 A4 00 00 02 3F 00/1001

#检查返回结果的byte[10](0x01则为普通地铁储值票，0x03则为员工卡）
#根据上面的检查结果，判定是进行储值票交易还是员工卡交易

#80 FA 00 XX 08 由应用序列号分散出消费子密钥

BUFF,0,{80 50 01 02 0B 01 00 00 00 BE $TERMINAL_ID 0F}

BUFF 0,4,OLD_BLANCE
BUFF 4,2,TRADE_SN
BUFF 6,3,OVERDRAFT_LIMIT
BUFF B,4,CARD_RAND_4BYTE

SET R2_SAM_TRADE_SN=00 AB 
SET SAM_TRADE_SN=00 00 00 AB 
SET SESS_KEY={TripDes($CARD_RAND_4BYTE  $TRADE_SN $R2_SAM_TRADE_SN ,$CPU_DPK )}

SET NOW={DATETIME}
BUFF,0,{KEY08MAC(00 00 00 BE 06 $TERMINAL_ID $NOW ,$SESS_KEY,0000000000000000  )}
BUFF 0,4,MAC1
#DEBIT FOR PURCHASE
BUFF,0,{80 54 01 00 0F $SAM_TRADE_SN $NOW $MAC1 08}
BUFF 0,4,TAC
BUFF 4,4,MAC2

################消费结束################### 
 
 
```