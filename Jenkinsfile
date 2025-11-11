pipeline {
    agent any

    environment {
        SONARQUBE = 'SonarQube'
        DOCKER_IMAGE = 'chaitanya1380/helloworldapp'
    }

    stages {

        stage('Restore') {
            steps {
                bat 'dotnet restore HelloWorldApp/HelloWorldApp.sln'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build HelloWorldApp/HelloWorldApp.sln --configuration Release'
            }
        }

        stage('SonarQube Analysis') {
    steps {
        withSonarQubeEnv('SonarQube') {
            bat '''
            SET PATH=%PATH%;C:\\Users\\chait\\.dotnet\\tools

            dotnet sonarscanner begin ^
              /k:"Chaitanya1380_Practice" ^
              /o:"chaitanya1380" ^
              /d:sonar.host.url="https://sonarcloud.io" ^
              /d:sonar.login="128b4f37e95dae351ab067aed7426e9588352d43" ^
              /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"

            dotnet build HelloWorldApp.sln --configuration Release

            dotnet sonarscanner end /d:sonar.login="128b4f37e95dae351ab067aed7426e9588352d43"
            '''
        }
    }
}
        stage('Publish') {
            steps {
                bat 'dotnet publish HelloWorldApp/HelloWorldApp.csproj -c Release -o published'
            }
        }

        stage('Docker Build & Push') {
            steps {
                script {
                    bat """
                        echo Building Docker image...
                        docker build -t %DOCKER_IMAGE%:latest .

                        echo Tagging image as version 1...
                        docker tag %DOCKER_IMAGE%:latest %DOCKER_IMAGE%:v1

                        echo Logging in to Docker...
                        docker login -u chaitanya1380 -p <YOUR_DOCKERHUB_TOKEN>

                        echo Pushing images to DockerHub...
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
