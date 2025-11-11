stage('SonarQube Analysis') {
    steps {
        dir('HelloWorldApp') {
            withSonarQubeEnv('SonarQube') {
                bat '''
                REM --- Ensure Jenkins can find the dotnet-sonarscanner tool ---
                SET PATH=%PATH%;C:\\Users\\chait\\.dotnet\\tools

                REM --- Start SonarQube Analysis ---
                dotnet sonarscanner begin ^
                  /k:"Chaitanya1380_Practice" ^
                  /o:"chaitanya1380" ^
                  /d:sonar.host.url="https://sonarcloud.io" ^
                  /d:sonar.login="128b4f37e95dae351ab067aed7426e9588352d43" ^
                  /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"

                REM --- Build the project (required for analysis) ---
                dotnet build HelloWorldApp.sln --configuration Release

                REM --- End SonarQube Analysis ---
                dotnet sonarscanner end /d:sonar.login="128b4f37e95dae351ab067aed7426e9588352d43"
                '''
            }
        }
    }
}
