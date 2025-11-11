pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/Chaitanya1380/Practice.git'
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish HelloWorldApp/HelloWorldApp.csproj -c Release -o ./publish'
            }
        }
    }

    post {
        always {
            archiveArtifacts artifacts: '**/publish/**/*.*', followSymlinks: false
        }
    }
}
