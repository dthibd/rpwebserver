Param(
    [Parameter(Mandatory)]
    [string]
    $ServerUrl,
    [Parameter(Mandatory)]
    [string]
    $RouteId
)

begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Route/$($RouteId)"
}

process {
    Invoke-RestMethod -Uri $ApiRoute -Method Delete
}
