#!/usr/bin/env pwsh
[CmdletBinding()]
Param (
    [string] $ImageName = 'microservice-beststories:local'
)
Begin {
    Push-Location (Join-Path $PSScriptRoot .. -Resolve)
}
Process {
    $ErrorActionPreference = 'Stop'

    $buildContext = Join-Path $PSScriptRoot .. -Resolve
    $dockerfilePath = Join-Path $buildContext './src/Microservice.BestStories/Dockerfile' -Resolve

    # build api image
    Write-Output "building docker image..."
    docker build -f $dockerfilePath -t $ImageName --target final --no-cache --force-rm `
        $BuildContext

    Write-Output "starting container..."
    docker run -d --name microservice-beststories -p 5000:80 $ImageName
}
End {
    Pop-Location
}
