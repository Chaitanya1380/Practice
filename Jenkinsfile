pipeline {
    agent any

    environment {
        SONARQUBE = 'SonarQube' // SonarQube name configured in Jenkins
        DOCKER_IMAGE = 'chaitanya1380/helloworldapp'
    }

    stages {
        stage('Restore') {
            steps {
                dir('HelloWorldApp') {
                    bat 'dotnet restore HelloWorldApp.sln'
                }
            }
        }

        stage('Build') {
            steps {
                dir('HelloWorldApp') {
                    bat 'dotnet build HelloWorldApp.sln --configuration Release'
                }
            }
        }

        stage('Test') {
            steps {
                dir('HelloWorldApp.Tests') {
                    bat 'dotnet test HelloWorldApp.Tests.csproj'
                }
            }
        }

        stage('SonarQube Analysis') {
    steps {
        withSonarQubeEnv('SonarQube') {
            bat '''
            REM --- Ensure Jenkins can find the dotnet-sonarscanner tool ---
            SET PATH=%PATH%;C:\\Users\\chait\\.dotnet\\tools

            REM --- Start SonarQube Analysis ---
            dotnet sonarscanner begin ^
              /k:"HelloWorldApp" ^
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

        stage('Publish') {
            steps {
                dir('HelloWorldApp') {
                    bat 'dotnet publish HelloWorldApp.csproj -c Release -o ../published'
                }
            }
        }

        stage('Docker Build & Push') {
            steps {
                script {
                    bat """
                        docker build -t %DOCKER_IMAGE%:latest .
                        docker tag %DOCKER_IMAGE%:latest %DOCKER_IMAGE%:v1
                        docker push %DOCKER_IMAGE%:latest
                        docker push %DOCKER_IMAGE%:v1
                    """
                }
            }
        }
    }

    post {
        always {
            archiveArtifacts artifacts: 'published/**', followSymlinks: false
        }
    }
}
