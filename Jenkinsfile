pipeline {
    agent any

    environment {
        SONARQUBE = 'SonarQube' // Must match the name configured in Jenkins "Manage Jenkins" > "Configure System"
        DOCKER_IMAGE = 'chaitanya1380/helloworldapp'
        DOCKERHUB_USERNAME = 'chaitanya1380'
        DOCKERHUB_TOKEN = credentials('dockerhub-token') // Jenkins credential ID (store your DockerHub token here)
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
                    // Generates code coverage for SonarQube
                    bat 'dotnet test HelloWorldApp.Tests.csproj --collect:"XPlat Code Coverage"'
                }
            }
        }

        stage('SonarQube Analysis') {
            steps {
                withSonarQubeEnv("${SONARQUBE}") {
                    bat '''
                    REM --- Add SonarScanner global path ---
                    SET PATH=%PATH%;C:\\Users\\chait\\.dotnet\\tools

                    REM --- Start SonarCloud Analysis ---
                    dotnet sonarscanner begin ^
                      /k:"chaitanyasiripurapu_Practice" ^
                      /o:"chaitanyasiripurapu" ^
                      /d:sonar.host.url="https://sonarcloud.io" ^
                      /d:sonar.login="128b4f37e95dae351ab067aed7426e9588352d43" ^
                      /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml" ^
                      /d:sonar.verbose=true

                    REM --- Build required inside scanner ---
                    dotnet build HelloWorldApp/HelloWorldApp.sln --configuration Release

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
                    // Login using Jenkins credentials
                    withCredentials([usernamePassword(credentialsId: 'dockerhub-token', usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD')]) {
                        bat """
                            docker build -t %DOCKER_IMAGE%:latest .
                            docker tag %DOCKER_IMAGE%:latest %DOCKER_IMAGE%:v1
                            docker login -u %USERNAME% -p %PASSWORD%
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
