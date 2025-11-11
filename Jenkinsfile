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

        stage('SonarQube Analysis + Test') {
            steps {
                dir('HelloWorldApp') {
                    withSonarQubeEnv('SonarQube') {
                        bat """
                        REM --- Ensure Jenkins can find the dotnet-sonarscanner tool ---
                        SET PATH=%PATH%;C:\\Users\\chait\\.dotnet\\tools

                        REM --- Begin SonarQube Analysis ---
                        dotnet sonarscanner begin ^
                          /k:"Chaitanya1380_Practice" ^
                          /o:"chaitanya1380" ^
                          /d:sonar.host.url="https://sonarcloud.io" ^
                          /d:sonar.login="128b4f37e95dae351ab067aed7426e9588352d43" ^
                          /d:sonar.cs.opencover.reportsPaths="../TestResults/coverage.opencover.xml"

                        REM --- Build the solution ---
                        dotnet build HelloWorldApp.sln --configuration Release
                        """

                        dir('HelloWorldApp.Tests') {
                            bat """
                            REM --- Run tests with coverage ---
                            dotnet test HelloWorldApp.Tests.csproj ^
                              /p:CollectCoverage=true ^
                              /p:CoverletOutputFormat=opencover ^
                              /p:CoverletOutput=../TestResults/coverage.opencover.xml
                            """
                        }

                        bat """
                        REM --- End SonarQube Analysis ---
                        dotnet sonarscanner end /d:sonar.login="128b4f37e95dae351ab067aed7426e9588352d43"
                        """
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
