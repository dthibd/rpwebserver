Param(
    [Parameter(Mandatory)]
    [string]
    $ServerUrl,
    [Parameter(Mandatory)]
    [Hashtable]
    $Route
)

begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Route"
    
}

process {
    $Route | ConvertTo-Json | Invoke-RestMethod -Uri $ApiRoute -Method Post -ContentType "application/json"
}


