Param(
    [Parameter(Mandatory)]
    [string]
    $ServerUrl
)

begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Refresh"

}

process {
    Invoke-RestMethod -Uri $ApiRoute -Method Post -ContentType "application/json"
}


