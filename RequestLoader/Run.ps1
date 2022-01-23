param ($url="http://localhost:8080/weatherforecast") 

$start = (Get-Date).ToFileTime();
$speed = 1000 * 1000 * 1000;

while (1 -eq 1) {  
    $x = ($start - (Get-Date).ToFileTime())/ $speed;
    $val = [math]::Round(10*(3 + 3 + [math]::Sin(2*$x) + [math]::Sin($x) + [math]::Sin($x/2)));
    for($i=0;$i -le $val;$i++) {
       Invoke-RestMethod -Uri $url | Out-Null     
    }   
    Write-Host "Requests $url invoked $val times"
}