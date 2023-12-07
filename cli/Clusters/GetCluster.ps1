Param(
    [Parameter(Mandatory)]
    [string]
    $ServerUrl,
    [Parameter(Mandatory)]
    [string]
    $ClusterId
)

begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Cluster/$($ClusterId)"
}

process {
    $Routes = Invoke-RestMethod -Uri $ApiRoute -Method Get

    $Routes
}
