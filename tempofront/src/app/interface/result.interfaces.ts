export interface IResult<T> {
  success: boolean;
  message: string;
  data: T;
  errors: string[] | null;
}