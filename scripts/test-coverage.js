const { exec } = require('child_process');
const os = require('os');

function removeCoverageFolder() {
    return new Promise((resolve, reject) => {

        var cmdLine = 'rm -rf ./coverage'

        if (os.platform() == 'win32') {
            reject('unsupported os');
        }

        var process = exec(cmdLine);

        process.on('exit', () => {
            resolve();
        });

    });
}

function collectTestCoverage() {
    return new Promise((resolve, reject) => {
        var process = exec('dotnet test WebComponentServerTest --collect:"XPlat Code Coverage" --results-directory:"./coverage/cobertura"');

        process.stdout.on('data', (data) => {
            if (data) {
                console.log(`${data.replaceAll('\r', '').replaceAll('\n', '')}`);
            }
        });

        process.on('exit', () => {
            resolve();
        });
    })
}

function buildCoverageReport() {
    return new Promise((resolve, reject) => {
        var process = exec('reportgenerator -targetdir:"./coverage/report" -reports:"./coverage/cobertura/**/coverage.cobertura.xml"');

        process.stdout.on('data', (data) => {
            if (data) {
                console.log(`${data.replaceAll('\r', '').replaceAll('\n', '')}`);
            }
        });

        process.on('exit', () => {
            resolve();
        });
    })
}

(async() => {

    await removeCoverageFolder();

    await collectTestCoverage();

    await buildCoverageReport();

    console.log('collection done');
})();


