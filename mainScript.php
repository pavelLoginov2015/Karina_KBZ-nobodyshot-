<?php
$baseNet = new mysqli('localhost','a1038531_madnessCubed3','tamplier153','a1038531_madnessCubed3');
class weaponInfo
{
    public function __construct(
    
    public string $weaponName,
    public int $priceType,
    public int $price,
    ){}
}

$Weapons = array(
   new weaponInfo('axe',0,0),
   new weaponInfo('uzi',0,500),
   new weaponInfo('shotgun',0,1500),
   new weaponInfo('obrez',1,2),
   new weaponInfo('avtomatm16',0,1000),
   new weaponInfo('pigal',0,250),
   new weaponInfo('sniperrifle',1,4),
   new weaponInfo('bazuka_1',1,8),
   new weaponInfo('kalash',1,2),
   new weaponInfo('sword',0,2000),
);

$REQUEST_GET = $_GET['requestCode'];
$REQUEST_POST = $_POST['requestCode'];
$UserId = $_GET['uid'];
$UserSecretKey_GET = $_GET['secret'];
$UserPlayerName = $_POST['nickName'];
$UserServerId_GET = $_GET['id'];
$UserServerId_POST = $_POST['id'];
$phpDate = date('Y-m-d');

function CheckMD($line,$requestType)
{
  $InstanceRequestParam = NULL;
    if ($requestType == 'get'){
      $InstanceRequestParam = $_GET['sig'];
    }else if ($requestType == 'post'){
      $InstanceRequestParam = $_POST['sig'];
    }
    if ($InstanceRequestParam == md5($line)){
      return TRUE;
    }
    return FALSE;
}

if ($REQUEST_GET == 1)
{ 
    $CheckAccountExistsFromUid= "SELECT * FROM playersData WHERE uid = '". $UserId ."'";
    $ResponseData = $baseNet->query($CheckAccountExistsFromUid)->fetch_assoc();
    if ($ResponseData == null)
    {
        $date_create = date("Y.m.d");
        $NewSecretToken = md5('kbs-2025'. rand(20,10000000000));
        $CreateAccountSQLInfo = "INSERT INTO playersData (uid, secretToken,secretKey,registerData) VALUES ('$UserId','$NewSecretToken','$UserSecretKey_GET','$date_create ')";
        $baseNet->query($CreateAccountSQLInfo);
        
         $SendReceiveData = "SELECT * FROM playersData WHERE uid = '". $UserId  ."'";
        $ResponseNewData = $baseNet->query($SendReceiveData)->fetch_assoc();
        $dataAtBytes = array();
        $fi_date = array(
          'b' => $ResponseNewData['fastInventar_weapons'],
          'a' => $ResponseNewData['fastInventar_creating'],
        ); 
        $GetDataFromJson = array('offer' => '[]',
        'sq' => $ResponseNewData,
        'wp_price' => $Weapons,
        't' => time(),
        'fi' => $fi_date,
        );
        echo json_encode ($GetDataFromJson);
    }
    else
    {  $fi_date = array(
          'b' => $ResponseData['fastInventar_weapons'],
          'a' => $ResponseData['fastInventar_creating'],
        ); 
         $GetDataFromJson = array('offer' => '[]',
        'sq' => $ResponseData,
        'wp_price' => $Weapons,
        't' => time(),
        'fi' => $fi_date,
        );
        echo json_encode ($GetDataFromJson);
    }
}

$FastInventoryData = $_POST['fi'];
$FastInventoryGroup = $_POST['group'];
if ($REQUEST_POST == 2)
{
    $groupNameTable = 'null+';
    if ($FastInventoryGroup == 0)
    {
        $groupNameTable = 'fastInventar_weapons';
    }
    else if ($FastInventoryGroup == 1){
        $groupNameTable = 'fastInventar_creating';
    }
    ChangeUserDBParameter('id',$UserServerId_POST,$groupNameTable,$FastInventoryData);
    echo '{ok}';
}
 $weaponId = $_GET['weaponnum'];
 if ($REQUEST_GET == 3)
 {
     $weaponCurrent = $Weapons[$weaponId];
     $weaponFromServer = GetUserDBParameter('id',$UserServerId_GET,$weaponCurrent->weaponName);
     $currentMoney = GetUserDBParameter('id',$UserServerId_GET,'money');
     $currentGold = GetUserDBParameter('id',$UserServerId_GET,'gold');
     if ($weaponCurrent->priceType == 0 && $currentMoney >= $weaponCurrent->price)
     {
        $currentMoney -= $weaponCurrent->price;  
        $weaponFromServer = 1; 
        ChangeUserDBParameter('id',$UserServerId_GET,$weaponCurrent->weaponName,$weaponFromServer);
        ChangeUserDBParameter('id',$UserServerId_GET,'money',$currentMoney);
     }
     else if ($weaponCurrent->priceType == 1 && $currentGold >= $weaponCurrent->price)
     {
        $currentGold -= $weaponCurrent->price;   
          $weaponFromServer = 1; 
        ChangeUserDBParameter('id',$UserServerId_GET,$weaponCurrent->weaponName,$weaponFromServer);
        ChangeUserDBParameter('id',$UserServerId_GET,'gold',$currentGold);
     }
     echo $currentMoney.'^'.$currentGold.'^'.$weaponId.'^'.$weaponFromServer;
 }

function GetUserDBParameter($variableName,$variableValue,$parameterSql)
{
   $GetReceiveParameter = "SELECT $parameterSql FROM playersData WHERE $variableName = '". $variableValue ."'";
   $localMySQLI = new mysqli('localhost','a1038531_madnessCubed3','tamplier153','a1038531_madnessCubed3');
   return $localMySQLI->query($GetReceiveParameter)->fetch_assoc()[$parameterSql];
}
function ChangeUserDBParameter($variableName,$variableValue,$parameterSql,$Value)
{
   $ChangeSqlParameter = "UPDATE playersData SET $parameterSql='$Value' WHERE $variableName='".$variableValue."'";
   $localMySQLI = new mysqli('localhost','a1038531_madnessCubed3','tamplier153','a1038531_madnessCubed3');
   return $localMySQLI->query($ChangeSqlParameter);
}
?>