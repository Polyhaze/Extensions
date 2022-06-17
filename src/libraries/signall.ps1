$list = (Get-ChildItem -Recurse -File -Path './' -Filter '*.csproj').FullName

foreach ($file in $list)
{
    $filePath = (Get-Item $file).Directory
    $keyPath = Join-Path -Path $filePath -ChildPath "key.snk"
    sn -k "$keyPath"
}
