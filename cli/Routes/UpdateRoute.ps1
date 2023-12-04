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
    $UpdatedRoute = $Route | ConvertTo-Json | Invoke-RestMethod -Uri $ApiRoute -Method Put -ContentType "application/json"

    $UpdatedRoute
}
