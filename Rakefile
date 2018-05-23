require 'rake'
require 'rake/clean'

# Configuration
PROJECT  = 'Cube.FileSystem.SevenZip'
BRANCHES = [ 'master', 'net35' ]
COPY     = 'cp -pf'
CHECKOUT = 'git checkout'
BUILD    = 'msbuild /t:Clean,Build /m /verbosity:minimal /p:Configuration=Release;Platform="Any CPU";GeneratePackageOnBuild=false'
RESTORE  = 'nuget restore'
PACK     = 'nuget pack -Properties "Configuration=Release;Platform=AnyCPU"'

# Tasks
task :default do
    Rake::Task[:clean].execute
    Rake::Task[:build].execute
    Rake::Task[:pack].execute
end

task :build do
    BRANCHES.each do |branch|
        sh("#{CHECKOUT} #{branch}")
        sh("#{RESTORE} #{PROJECT}.sln")
        sh("#{BUILD} #{PROJECT}.sln")
    end
end

task :pack do
    sh("#{CHECKOUT} net35")
    sh("#{PACK} Libraries/#{PROJECT}.nuspec")
    sh("#{CHECKOUT} master")
end

CLEAN.include("#{PROJECT}.*.nupkg")
