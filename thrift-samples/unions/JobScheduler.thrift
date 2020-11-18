namespace netstd Thrift.Net.Examples.Unions

struct JobRunInfo {
    1: string StartTime
    2: string EndTime
}

union JobSchedule {
    1: i32 maxDuration,
    2: JobRunInfo RunInfo
}

struct CreateJobRequest {
    1: string Name
    2: JobSchedule SchedulingDetails
}
