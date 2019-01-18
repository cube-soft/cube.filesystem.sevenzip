require 'rake'
require 'rake/clean'
require 'fileutils'

# --------------------------------------------------------------------------- #
# Configuration
# --------------------------------------------------------------------------- #
SOLUTION       = 'Cube.FileSystem.SevenZip'
BRANCHES       = [ 'master', 'net35' ]
PLATFORMS      = [ 'x86', 'x64' ]
CONFIGURATIONS = [ 'Debug', 'Release' ]
NATIVE         = '../resources/native'

# --------------------------------------------------------------------------- #
# Commands
# --------------------------------------------------------------------------- #
CHECKOUT = 'git checkout'
BUILD    = 'msbuild /t:Clean,Build /m /verbosity:minimal /p:Configuration=Release;Platform="Any CPU";GeneratePackageOnBuild=false'
RESTORE  = 'nuget restore'
PACK     = 'nuget pack -Properties "Configuration=Release;Platform=AnyCPU"'

# --------------------------------------------------------------------------- #
# Functions
# --------------------------------------------------------------------------- #
def do_copy(src, dest)
    FileUtils.mkdir_p(dest)
    FileUtils.cp_r(src, dest)
end

# --------------------------------------------------------------------------- #
# Tasks
# --------------------------------------------------------------------------- #
task :default do
    Rake::Task[:clean].execute
    Rake::Task[:build].execute
    Rake::Task[:copy].execute
    Rake::Task[:pack].execute
end

# --------------------------------------------------------------------------- #
# Build
# --------------------------------------------------------------------------- #
task :build do
    BRANCHES.each { |branch|
        sh("#{CHECKOUT} #{branch}")
        sh("#{RESTORE} #{SOLUTION}.sln")
        sh("#{BUILD} #{SOLUTION}.sln")
    }
end

# --------------------------------------------------------------------------- #
# Build
# --------------------------------------------------------------------------- #
task :pack do
    sh("#{CHECKOUT} net35")
    sh("#{PACK} Libraries/#{SOLUTION}.nuspec")
    sh("#{CHECKOUT} master")
end

# --------------------------------------------------------------------------- #
# Copy
# --------------------------------------------------------------------------- #
task :copy do
    [ '', 'net35' ].product(PLATFORMS, CONFIGURATIONS) { |set|
        x86_64  = [ 'bin', set[0], set[1], set[2] ].compact.reject(&:empty?).join('/')
        any_cpu = [ 'bin', set[0], set[2] ].compact.reject(&:empty?).join('/')

        [ 'Tests', 'Applications/Ice/Tests', 'Applications/Ice/Progress' ].each { |dest|
            src  = [ NATIVE, set[1] ].join('/')
            do_copy("#{src}/7z.dll", "#{dest}/#{x86_64}")
            do_copy("#{src}/7z.dll", "#{dest}/#{any_cpu}") if (set[1] == 'x64')
            do_copy("#{src}/7z.sfx", "#{dest}/#{x86_64}")
            do_copy("#{src}/7z.sfx", "#{dest}/#{any_cpu}") if (set[1] == 'x64')
        }
    }
end

# --------------------------------------------------------------------------- #
# Clean
# --------------------------------------------------------------------------- #
CLEAN.include("#{SOLUTION}.*.nupkg")
CLEAN.include(%w{dll log}.map{ |e| "**/*.#{e}" })