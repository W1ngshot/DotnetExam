export interface ApiError {
    title: string;
    status: number;
    placeholder: Record<string, string>
    traceId: string;
  }