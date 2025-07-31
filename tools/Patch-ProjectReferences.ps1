$pattern1 = '..\..\UIBaseClass\UIBaseClass\UIBaseClass.csproj'
$pattern2 = '..\..\..\Custom-Error-Message-Box\CustomErrorMessageBox\CustomErrorMessageBox.csproj'
$pattern3 = '..\..\Custom-Error-Message-Box\CustomErrorMessageBox\CustomErrorMessageBox.csproj'

$replace1 = '..\UIBaseClass\UIBaseClass\UIBaseClass.csproj'
$replace2 = '..\..\Custom-Error-Message-Box\CustomErrorMessageBox\CustomErrorMessageBox.csproj'
$replace3 = '..\Custom-Error-Message-Box\CustomErrorMessageBox\CustomErrorMessageBox.csproj'


$csprojFiles = Get-ChildItem -Recurse -Filter *.csproj
$slnFiles = Get-ChildItem -Recurse -Filter *.sln

foreach ($file in $csprojFiles) {
    $content = Get-Content $file.FullName -Raw
    $patched = $content.Replace($pattern1, $replace1).Replace($pattern2, $replace2).Replace($pattern3, $replace3)

    if ($patched -ne $content) {
        Write-Host "Patching: $($file.FullName)"
        Write-Host "`nUpdated content for: $($file.FullName)"
        Write-Host $patched
        Set-Content $file.FullName $patched
    }
}

foreach ($file in $slnFiles) {
    $content = Get-Content $file.FullName -Raw
    $patched = $content.Replace($pattern1, $replace1)

    if ($patched -ne $content) {
        Write-Host "Patching: $($file.FullName)"
        Write-Host "`nUpdated content for: $($file.FullName)"
        Write-Host $patched
        Set-Content $file.FullName $patched
    }
}
