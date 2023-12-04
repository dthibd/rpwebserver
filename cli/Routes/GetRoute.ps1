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
    $Route = Invoke-RestMethod -Uri $ApiRoute -Method Get

    $Route
}
