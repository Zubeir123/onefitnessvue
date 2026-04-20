using System.Data;
using FitnessTimeGym.Common;
using FitnessTimeGym.Model.MemberRegistration;
using FitnessTimeGym.Model.PaymentDetails;
using FitnessTimeGym.WinForms.Models;
using Microsoft.Data.SqlClient;

namespace FitnessTimeGym.WinForms.Data;

public class GymRepository
{
    private readonly DatabaseHelper _databaseHelper;

    public GymRepository(DatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public string GenerateMemberNo()
    {
        var keyGenerator = new KeyGenerator();
        return $"OFV{DateTime.Now.DayOfYear}{keyGenerator.GetUniqueKey(10).ToUpperInvariant()}";
    }

    public string GenerateLoginToken()
    {
        return Guid.NewGuid().ToString("N");
    }

    public bool CheckUsernameExists(string username)
    {
        const string sql = "SELECT COUNT(1) FROM Usermaster WHERE UserName = @UserName";
        var table = _databaseHelper.ExecuteQuery(sql, new[]
        {
            new SqlParameter("@UserName", username)
        });
        return table.Rows.Count > 0 && Convert.ToInt32(table.Rows[0][0]) > 0;
    }

    public string? GetPasswordHash(string username)
    {
        const string sql = "SELECT TOP 1 PasswordHash FROM Usermaster WHERE UserName = @UserName";
        var table = _databaseHelper.ExecuteQuery(sql, new[]
        {
            new SqlParameter("@UserName", username)
        });

        return table.Rows.Count == 0 ? null : Convert.ToString(table.Rows[0]["PasswordHash"]);
    }

    public UserSession? GetUserSession(string username)
    {
        const string sql = """
                           SELECT TOP 1 UserId, UserName, FirstName, EmailId, MobileNo, Status
                           FROM Usermaster
                           WHERE UserName = @UserName
                           """;
        var table = _databaseHelper.ExecuteQuery(sql, new[]
        {
            new SqlParameter("@UserName", username)
        });

        if (table.Rows.Count == 0)
        {
            return null;
        }

        var row = table.Rows[0];
        return new UserSession
        {
            UserId = Convert.ToInt32(row["UserId"]),
            UserName = Convert.ToString(row["UserName"]) ?? string.Empty,
            FirstName = Convert.ToString(row["FirstName"]) ?? string.Empty,
            EmailId = Convert.ToString(row["EmailId"]) ?? string.Empty,
            MobileNo = Convert.ToString(row["MobileNo"]) ?? string.Empty,
            Status = Convert.ToBoolean(row["Status"])
        };
    }

    public List<LookupItem> GetMembershipTypes()
    {
        const string sql = """
                           SELECT MembershipTypeId AS Id, MembershipTypeName AS Name, Amount
                           FROM MembershipTypes
                           WHERE Status = 1
                           ORDER BY MembershipTypeName
                           """;
        return ToLookupItems(_databaseHelper.ExecuteQuery(sql), includeAmount: true);
    }

    public List<LookupItem> GetWorkouts()
    {
        const string sql = """
                           SELECT WorkOutId AS Id, WorkOutName AS Name
                           FROM WorkOuts
                           WHERE Status = 1
                           ORDER BY WorkOutName
                           """;
        return ToLookupItems(_databaseHelper.ExecuteQuery(sql));
    }

    public List<LookupItem> GetInstallments()
    {
        const string sql = """
                           SELECT InstallmentId AS Id, InstallmentName AS Name, InstallmentMonths AS Months
                           FROM Installments
                           WHERE Status = 1
                           ORDER BY InstallmentId
                           """;
        return ToLookupItems(_databaseHelper.ExecuteQuery(sql), includeMonths: true);
    }

    public List<LookupItem> GetPaymentTypes()
    {
        return new List<LookupItem>
        {
            new() { Id = 1, Name = "Cash" }
        };
    }

    public List<LookupItem> GetTaxTypes()
    {
        return new List<LookupItem>
        {
            new() { Id = 1, Name = "GST", Amount = 10m },
            new() { Id = 2, Name = "VAT", Amount = 12m }
        };
    }

    public bool CheckMemberMobileNoExists(string mobileNo)
    {
        const string sql = "SELECT COUNT(1) FROM MemberRegistration WHERE MobileNo = @MobileNo";
        var table = _databaseHelper.ExecuteQuery(sql, new[]
        {
            new SqlParameter("@MobileNo", mobileNo)
        });
        return table.Rows.Count > 0 && Convert.ToInt32(table.Rows[0][0]) > 0;
    }

    public bool CheckMemberEmailExists(string emailId)
    {
        const string sql = "SELECT COUNT(1) FROM MemberRegistration WHERE EmailId = @EmailId";
        var table = _databaseHelper.ExecuteQuery(sql, new[]
        {
            new SqlParameter("@EmailId", emailId)
        });
        return table.Rows.Count > 0 && Convert.ToInt32(table.Rows[0][0]) > 0;
    }

    public decimal GetMembershipAmount(int membershipTypeId)
    {
        const string sql = "SELECT TOP 1 Amount FROM MembershipTypes WHERE MembershipTypeId = @MembershipTypeId";
        var table = _databaseHelper.ExecuteQuery(sql, new[]
        {
            new SqlParameter("@MembershipTypeId", membershipTypeId)
        });
        return table.Rows.Count == 0 ? 0m : Convert.ToDecimal(table.Rows[0]["Amount"]);
    }

    public int GetInstallmentMonths(int installmentId)
    {
        const string sql = "SELECT TOP 1 InstallmentMonths FROM Installments WHERE InstallmentId = @InstallmentId";
        var table = _databaseHelper.ExecuteQuery(sql, new[]
        {
            new SqlParameter("@InstallmentId", installmentId)
        });
        return table.Rows.Count == 0 ? 0 : Convert.ToInt32(table.Rows[0]["InstallmentMonths"]);
    }

    public decimal GetTaxPercentage(int taxId)
    {
        return taxId switch
        {
            1 => 10m,
            2 => 12m,
            _ => 0m
        };
    }

    public long GetNextInvoiceNo()
    {
        using var connection = _databaseHelper.CreateConnection();
        connection.Open();

        using var command = new SqlCommand("Usp_GetNewInvoiceId", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        var outputParameter = new SqlParameter("@InvoiceId", SqlDbType.BigInt)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(outputParameter);

        try
        {
            var result = command.ExecuteNonQuery();
            if (result >= 0 && outputParameter.Value != DBNull.Value)
            {
                return Convert.ToInt64(outputParameter.Value);
            }
        }
        catch
        {
            // Fall back to max invoice no for environments without stored procedure.
        }

        const string fallbackSql = "SELECT ISNULL(MAX(InvoiceNo), 0) + 1 AS NextInvoiceNo FROM PaymentDetails";
        var table = _databaseHelper.ExecuteQuery(fallbackSql);
        return table.Rows.Count == 0 ? 1 : Convert.ToInt64(table.Rows[0]["NextInvoiceNo"]);
    }

    public bool AddMemberWithPayment(MemberRegistrationModel member, PaymentDetailsModel payment)
    {
        using var connection = _databaseHelper.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            const string insertMemberSql = """
                                           INSERT INTO MemberRegistration
                                           (MemberNo, FirstName, LastName, MiddleName, DOB, Age, MobileNo, EmailId, GenderId, Address,
                                            JoiningDate, CreatedOn, CreatedBy, Status, EmergencyContactName, EmergencyContactNo)
                                           VALUES
                                           (@MemberNo, @FirstName, @LastName, @MiddleName, @DOB, @Age, @MobileNo, @EmailId, @GenderId, @Address,
                                            @JoiningDate, @CreatedOn, @CreatedBy, @Status, @EmergencyContactName, @EmergencyContactNo);
                                           SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS MemberId;
                                           """;

            var memberResult = _databaseHelper.ExecuteQuery(insertMemberSql, new[]
            {
                new SqlParameter("@MemberNo", member.MemberNo),
                new SqlParameter("@FirstName", member.FirstName),
                new SqlParameter("@LastName", member.LastName),
                new SqlParameter("@MiddleName", string.IsNullOrWhiteSpace(member.MiddleName) ? DBNull.Value : member.MiddleName),
                new SqlParameter("@DOB", (object?)member.DOB ?? DBNull.Value),
                new SqlParameter("@Age", member.Age),
                new SqlParameter("@MobileNo", member.MobileNo),
                new SqlParameter("@EmailId", string.IsNullOrWhiteSpace(member.EmailId) ? DBNull.Value : member.EmailId),
                new SqlParameter("@GenderId", member.GenderId),
                new SqlParameter("@Address", member.Address),
                new SqlParameter("@JoiningDate", (object?)member.JoiningDate ?? DBNull.Value),
                new SqlParameter("@CreatedOn", member.CreatedOn ?? DateTime.Now),
                new SqlParameter("@CreatedBy", member.CreatedBy ?? 0),
                new SqlParameter("@Status", member.Status),
                new SqlParameter("@EmergencyContactName", member.EmergencyContactName),
                new SqlParameter("@EmergencyContactNo", member.EmergencyContactNo)
            }, transaction);

            if (memberResult.Rows.Count == 0)
            {
                transaction.Rollback();
                return false;
            }

            var memberId = Convert.ToInt64(memberResult.Rows[0]["MemberId"]);
            payment.MemberID = memberId;
            var paymentInserted = InsertPayment(payment, transaction);
            if (!paymentInserted)
            {
                transaction.Rollback();
                return false;
            }

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public bool AddPayment(PaymentDetailsModel payment)
    {
        using var connection = _databaseHelper.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            var inserted = InsertPayment(payment, transaction);
            if (!inserted)
            {
                transaction.Rollback();
                return false;
            }

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public DataTable SearchMembers(string searchText)
    {
        const string sql = """
                           SELECT
                               MemberId,
                               MemberNo,
                               CONCAT(FirstName, ' ', ISNULL(MiddleName, ''), ' ', LastName) AS FullName,
                               MobileNo,
                               EmailId,
                               JoiningDate,
                               CASE WHEN Status = 1 THEN 'Active' ELSE 'InActive' END AS Status
                           FROM MemberRegistration
                           WHERE (@SearchText = '' OR MemberNo LIKE '%' + @SearchText + '%' OR FirstName LIKE '%' + @SearchText + '%')
                           ORDER BY MemberId DESC
                           """;
        return _databaseHelper.ExecuteQuery(sql, new[]
        {
            new SqlParameter("@SearchText", searchText ?? string.Empty)
        });
    }

    public DataTable SearchPayments(string searchText)
    {
        const string sql = """
                           SELECT
                               p.PaymentID,
                               m.MemberNo,
                               CONCAT(m.FirstName, ' ', ISNULL(m.MiddleName, ''), ' ', m.LastName) AS FullName,
                               p.PaymentFromdt,
                               p.PaymentTodt,
                               p.NextRenewalDate,
                               p.Amount,
                               p.TaxPercentage,
                               p.TotalAmount,
                               CASE p.ApplicationType
                                   WHEN 'NW' THEN 'New'
                                   WHEN 'RE' THEN 'Renewed'
                                   WHEN 'RF' THEN 'Refund'
                                   ELSE '--'
                               END AS ApplicationType
                           FROM PaymentDetails p
                           INNER JOIN MemberRegistration m ON p.MemberID = m.MemberId
                           WHERE (@SearchText = '' OR m.MemberNo LIKE '%' + @SearchText + '%' OR m.FirstName LIKE '%' + @SearchText + '%')
                           ORDER BY p.CreatedOn DESC
                           """;
        return _databaseHelper.ExecuteQuery(sql, new[]
        {
            new SqlParameter("@SearchText", searchText ?? string.Empty)
        });
    }

    public DataTable GetRenewalReport(DateTime fromDate, DateTime toDate)
    {
        using var connection = _databaseHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand("Usp_GetAllRenwalrecordsFromBetweenDate", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Paymentfromdt", fromDate.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@Paymenttodt", toDate.ToString("yyyy-MM-dd"));

        var table = new DataTable();
        try
        {
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            return table;
        }
        catch
        {
            const string fallbackSql = """
                                       SELECT
                                           m.MemberNo,
                                           CONCAT(m.FirstName, ' ', ISNULL(m.MiddleName, ''), ' ', m.LastName) AS Name,
                                           i.InstallmentName,
                                           mt.MembershipTypeName,
                                           w.WorkOutName,
                                           CONVERT(varchar(10), m.JoiningDate, 23) AS JoiningDate,
                                           CONVERT(varchar(10), p.NextRenewalDate, 23) AS RenewalDate,
                                           CAST(p.TotalAmount AS varchar(50)) AS TotalAmount,
                                           m.MobileNo,
                                           m.EmailId AS EmailID,
                                           m.Address
                                       FROM PaymentDetails p
                                       INNER JOIN MemberRegistration m ON p.MemberID = m.MemberId
                                       LEFT JOIN Installments i ON p.InstallmentId = i.InstallmentId
                                       LEFT JOIN MembershipTypes mt ON p.MembershipTypeId = mt.MembershipTypeId
                                       LEFT JOIN WorkOuts w ON p.WorkOutId = w.WorkOutId
                                       WHERE p.PaymentFromdt >= @FromDate AND p.PaymentTodt <= @ToDate
                                       ORDER BY p.PaymentFromdt DESC
                                       """;

            return _databaseHelper.ExecuteQuery(fallbackSql, new[]
            {
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate)
            });
        }
    }

    public DataRow? GetMemberByMemberNo(string memberNo)
    {
        const string sql = """
                           SELECT TOP 1 MemberId, MemberNo, FirstName, LastName, MiddleName, JoiningDate
                           FROM MemberRegistration
                           WHERE MemberNo = @MemberNo
                           """;
        var table = _databaseHelper.ExecuteQuery(sql, new[]
        {
            new SqlParameter("@MemberNo", memberNo)
        });
        return table.Rows.Count == 0 ? null : table.Rows[0];
    }

    private bool InsertPayment(PaymentDetailsModel payment, SqlTransaction transaction)
    {
        const string insertPaymentSql = """
                                        INSERT INTO PaymentDetails
                                        (MembershipTypeId, WorkOutId, PaymentFromdt, PaymentTodt, NextRenewalDate, CreatedOn, CreatedBy,
                                         ModifiedOn, ModifiedBy, RecStatus, MemberID, PaymentTypeId, TaxId, Amount, TaxPercentage,
                                         TotalAmount, MemberNo, InstallmentId, TaxPercentageAmount, ApplicationType, InvoiceNo)
                                        VALUES
                                        (@MembershipTypeId, @WorkOutId, @PaymentFromdt, @PaymentTodt, @NextRenewalDate, @CreatedOn, @CreatedBy,
                                         @ModifiedOn, @ModifiedBy, @RecStatus, @MemberID, @PaymentTypeId, @TaxId, @Amount, @TaxPercentage,
                                         @TotalAmount, @MemberNo, @InstallmentId, @TaxPercentageAmount, @ApplicationType, @InvoiceNo)
                                        """;

        var affectedRows = _databaseHelper.ExecuteNonQuery(insertPaymentSql, new[]
        {
            new SqlParameter("@MembershipTypeId", (object?)payment.MembershipTypeId ?? DBNull.Value),
            new SqlParameter("@WorkOutId", (object?)payment.WorkOutId ?? DBNull.Value),
            new SqlParameter("@PaymentFromdt", (object?)payment.PaymentFromdt ?? DBNull.Value),
            new SqlParameter("@PaymentTodt", (object?)payment.PaymentTodt ?? DBNull.Value),
            new SqlParameter("@NextRenewalDate", (object?)payment.NextRenewalDate ?? DBNull.Value),
            new SqlParameter("@CreatedOn", payment.CreatedOn ?? DateTime.Now),
            new SqlParameter("@CreatedBy", payment.CreatedBy ?? 0),
            new SqlParameter("@ModifiedOn", (object?)payment.ModifiedOn ?? DBNull.Value),
            new SqlParameter("@ModifiedBy", (object?)payment.ModifiedBy ?? DBNull.Value),
            new SqlParameter("@RecStatus", payment.RecStatus ?? "A"),
            new SqlParameter("@MemberID", (object?)payment.MemberID ?? DBNull.Value),
            new SqlParameter("@PaymentTypeId", (object?)payment.PaymentTypeId ?? DBNull.Value),
            new SqlParameter("@TaxId", (object?)payment.TaxId ?? DBNull.Value),
            new SqlParameter("@Amount", payment.Amount),
            new SqlParameter("@TaxPercentage", (object?)payment.TaxPercentage ?? DBNull.Value),
            new SqlParameter("@TotalAmount", payment.TotalAmount),
            new SqlParameter("@MemberNo", string.IsNullOrWhiteSpace(payment.MemberNo) ? DBNull.Value : payment.MemberNo),
            new SqlParameter("@InstallmentId", (object?)payment.InstallmentId ?? DBNull.Value),
            new SqlParameter("@TaxPercentageAmount", (object?)payment.TaxPercentageAmount ?? DBNull.Value),
            new SqlParameter("@ApplicationType", payment.ApplicationType ?? "NW"),
            new SqlParameter("@InvoiceNo", (object?)payment.InvoiceNo ?? DBNull.Value)
        }, transaction);

        return affectedRows > 0;
    }

    private static List<LookupItem> ToLookupItems(DataTable table, bool includeMonths = false, bool includeAmount = false)
    {
        var items = new List<LookupItem>();
        foreach (DataRow row in table.Rows)
        {
            var item = new LookupItem
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = Convert.ToString(row["Name"]) ?? string.Empty
            };

            if (includeMonths && table.Columns.Contains("Months"))
            {
                item.Months = Convert.ToInt32(row["Months"]);
            }

            if (includeAmount && table.Columns.Contains("Amount"))
            {
                item.Amount = Convert.ToDecimal(row["Amount"]);
            }

            items.Add(item);
        }

        return items;
    }
}
