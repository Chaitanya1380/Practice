pipeline {
    agent any

    environment {
        SONARQUBE = 'SonarQube' // As configured in Jenkins Global Tool Config
        DOCKER_IMAGE = 'helloworldapp' // Only local image build (not pushing to DockerHub)
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

        stage('Quality Gate') {
            steps {
                timeout(time: 2, unit: 'MINUTES') {
                    waitForQualityGate abortPipeline: true
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
        withCredentials([usernamePassword(credentialsId: 'dockerhub-cred', 
                                          usernameVariable: 'DOCKER_USERNAME', 
                                          passwordVariable: 'DOCKER_PASSWORD')]) {
            script {
                bat """
                    echo Building Docker image...
                    set IMAGE_NAME=%DOCKER_USERNAME%/helloworldapp

                    docker build -t %IMAGE_NAME%:latest .

                    echo Tagging image as version 1...
                    docker tag %IMAGE_NAME%:latest %IMAGE_NAME%:v1

                    echo Logging in to Docker Hub...
                    docker login -u %DOCKER_USERNAME% -p %DOCKER_PASSWORD%

                    echo Pushing images to Docker Hub...
                    docker push %IMAGE_NAME%:latest
                    docker push %IMAGE_NAME%:v1
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
