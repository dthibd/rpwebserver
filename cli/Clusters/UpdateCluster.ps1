Param(
    [Parameter(Mandatory)]
    [string]
    $ServerUrl,
    [Parameter(Mandatory)]
    [Hashtable]
    $Cluster
)

begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Cluster"
}

process {
    $UpdatedRoute = $Cluster | ConvertTo-Json | Invoke-RestMethod -Uri $ApiRoute -Method Put -ContentType "application/json"

    $UpdatedRoute
}
