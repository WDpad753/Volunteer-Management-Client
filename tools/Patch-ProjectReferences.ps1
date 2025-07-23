$pattern1 = '..\..\UIBaseClass\UIBaseClass\UIBaseClass.csproj'

$replace1 = '..\UIBaseClass\UIBaseClass\UIBaseClass.csproj'


$csprojFiles = Get-ChildItem -Recurse -Filter *.csproj
$slnFiles = Get-ChildItem -Recurse -Filter *.sln

foreach ($file in $csprojFiles) {
    $content = Get-Content $file.FullName -Raw
    $patched = $content.Replace($pattern1, $replace1)

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
