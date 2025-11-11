pipeline {
    agent any

    environment {
        SONARQUBE = 'SonarQube' // Name configured in Jenkins Global Tool Config
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
                    bat '''
                        dotnet test HelloWorldApp.Tests.csproj ^
                        /p:CollectCoverage=true ^
                        /p:CoverletOutputFormat=opencover ^
                        /p:CoverletOutput=../TestResults/coverage.opencover.xml
                    '''
                }
            }
        }

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
            withCredentials([usernamePassword(credentialsId: 'dockerhub-cred', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
                bat """
                    echo Building Docker image...
                    docker build -t %DOCKER_IMAGE%:latest .

                    echo Tagging image as version 1...
                    docker tag %DOCKER_IMAGE%:latest %DOCKER_IMAGE%:v1

                    echo Logging in to DockerHub...
                    docker login -u %DOCKER_USER% -p %DOCKER_PASS%

                    echo Pushing images...
                    docker push %DOCKER_IMAGE%:latest
                    docker push %DOCKER_IMAGE%:v1
                """
            }
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
