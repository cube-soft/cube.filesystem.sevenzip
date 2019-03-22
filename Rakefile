require 'rake'
require 'rake/clean'
require 'fileutils'

# --------------------------------------------------------------------------- #
# Configuration
# --------------------------------------------------------------------------- #
SOLUTION  = 'Cube.FileSystem.SevenZip'
SUFFIX    = 'Ice'
NATIVE    = '../resources/native'
BRANCHES  = [ 'stable', 'net35' ]
PLATFORMS = [ 'Any CPU', 'x86', 'x64' ]
CONFIGS   = [ 'Release', 'Debug' ]
TESTTOOLS = [ 'NUnit.ConsoleRunner', 'OpenCover', 'ReportGenerator' ]
TESTCASES = {
    'Cube.FileSystem.SevenZip.Tests'     => 'Tests',
    'Cube.FileSystem.SevenZip.Ice.Tests' => 'Applications/Ice/Tests'
}

# --------------------------------------------------------------------------- #
# Commands
# --------------------------------------------------------------------------- #
BUILD = 'msbuild /t:Clean,Build /m /verbosity:minimal /p:Configuration=Release;Platform="Any CPU";GeneratePackageOnBuild=false'
PACK  = 'nuget pack -Properties "Configuration=Release;Platform=AnyCPU"'
TEST  = '../packages/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe'

# --------------------------------------------------------------------------- #
# Tasks
# --------------------------------------------------------------------------- #
task :default do
    Rake::Task[:clean].execute
    Rake::Task[:build].execute
    Rake::Task[:pack].execute
end

# --------------------------------------------------------------------------- #
# Restore
# --------------------------------------------------------------------------- #
task :restore do
    sh("nuget restore #{SOLUTION}.#{SUFFIX}.sln")
    TESTTOOLS.each { |src| sh("nuget install #{src}") }
end

# --------------------------------------------------------------------------- #
# Build
# --------------------------------------------------------------------------- #
task :build do
    BRANCHES.each { |branch|
        sh("git checkout #{branch}")
        Rake::Task[:restore].execute
        sh("#{BUILD} #{SOLUTION}.#{SUFFIX}.sln")
    }
end

# --------------------------------------------------------------------------- #
# Pack
# --------------------------------------------------------------------------- #
task :pack do
    sh("git checkout net35")
    sh("#{PACK} Libraries/#{SOLUTION}.nuspec")
    sh("git checkout master")
end

# --------------------------------------------------------------------------- #
# Copy
# --------------------------------------------------------------------------- #
task :copy do
    [ 'net45', 'net35' ].product(PLATFORMS, CONFIGS) { |set|
        pf  = (set[1] == 'Any CPU') ? 'x64' : set[1]
        bin = [ 'bin', set[1], set[2], set[0] ].join('/')
        [ 'Tests', 'Applications/Ice/Tests', 'Applications/Ice/Progress' ].each { |dest|
            FileUtils.mkdir_p("#{dest}/#{bin}")
            FileUtils.cp_r(Dir.glob("#{NATIVE}/#{pf}/7z/7z.*"), "#{dest}/#{bin}")
        }
    }
end

# --------------------------------------------------------------------------- #
# Test
# --------------------------------------------------------------------------- #
task :test do
    Rake::Task[:restore].execute
    sh("#{BUILD} #{SOLUTION}.#{SUFFIX}.sln")

    fw  = `git symbolic-ref --short HEAD`.chomp
    fw  = 'net45' if (fw != 'net35')
    bin = [ 'bin', PLATFORMS[0], CONFIGS[0], fw ].join('/')
    TESTCASES.each { |proj, dir| sh("#{TEST} \"#{dir}/#{bin}/#{proj}.dll\"") }
end

# --------------------------------------------------------------------------- #
# Clean
# --------------------------------------------------------------------------- #
CLEAN.include("#{SOLUTION}.*.nupkg")
CLEAN.include(%w{bin obj}.map{ |e| "**/#{e}/*" })